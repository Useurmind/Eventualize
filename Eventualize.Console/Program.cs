using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using Autofac;

using Dapper;

using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;

using Eventualize.Autofac.Infrastructure;
using Eventualize.Console.Domain;
using Eventualize.Console.ReadModel;
using Eventualize.Dapper.Materialization;
using Eventualize.Domain;
using Eventualize.Domain.Core;
using Eventualize.EventStore.Infrastructure;
using Eventualize.Infrastructure;
using Eventualize.Materialization;
using Eventualize.NEventStore.Infrastructure;
using Eventualize.NEventStore.Materialization;
using Eventualize.NEventStore.Persistence;
using Eventualize.Persistence;

using NEventStore;

namespace Eventualize.Console
{
    class Program
    {
        private static IAggregateRepository aggregateRepository;

        private static InMemoryMaterialization materializationStrategy;

        private static IContainer container;

        static void Main(string[] args)
        {
            container = SetupContainer(true);
            using (container)
            {
                var eventContainer = container.Eventualize();

                eventContainer.MaterializationEventPoller.Run();

                aggregateRepository = eventContainer.AggregateRepository;
                //materializationStrategy = eventContainer.AggregateMaterializationStrategies.OfType<InMemoryMaterialization>().First();

                while (true)
                {
                    System.Console.WriteLine("Enter a command:");

                    var command = System.Console.ReadLine();

                    HandleCommand(command);
                }
            }
        }

        private static IContainer SetupContainer(bool forEventStore)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new Func<IDbConnection>(() => new SqlConnection(@"Server=.\SQLEXPRESS;Database=EventualizeTest;Integrated Security=True;")))
                   .As<Func<IDbConnection>>();

            builder.Register(
                c =>
                    new SimpleAggregateMaterializer<Task, TaskReadModel>(
                        c.Resolve<Func<IDbConnection>>(),
                        (task, @event) => new TaskReadModel()
                        {
                            Id = task.Id,
                            Title = task.Title,
                            Description = task.Description,
                            Version = task.Version
                        })).As<IAggregateMaterializer>();

            builder.Eventualize(
                b =>
                {
                    b.SetDefaults(Assembly.GetExecutingAssembly());
                    b.MaterializePerAggregate();
                });

            if (forEventStore)
            {
                builder.Eventualize(
                    b =>
                    {
                        //b.RegisterSingleInstance(
                        //     c =>
                        //     {
                        //         var notListening = new IPEndPoint(IPAddress.None, 0);

                        //         var node = EmbeddedVNodeBuilder.AsSingleNode()
                        //             .RunInMemory()
                        //             .WithExternalHttpOn(notListening)
                        //             .WithInternalHttpOn(notListening)
                        //             .WithExternalTcpOn(notListening)
                        //             .WithInternalTcpOn(notListening)
                        //             .Build();

                        //         node.StartAndWaitUntilReady();
                        //         return node;
                        //     })
                        b.ConnectEventStore(new Uri(@"tcp://admin:changeit@127.0.0.1:1113"), ConnectionSettings.Default)
                        .StoreAggregatesInEventStore()
                        .MaterializeFromEventStore();
                    });
            }
            else
            {
                builder.Eventualize(
                    b =>
                    {
                        b.ConnectNEventStore(c => CreateMemoryConnection())
                         .StoreAggregatesInNEventStore()
                         .MaterializeFromNEventStore();
                    });
            }

            return builder.Build();
        }

        static void HandleCommand(string command)
        {
            var commandParts = command.Split(' ');
            var commandName = commandParts[0];
            var commandArguments = commandParts.Skip(1);

            switch (commandName)
            {
                case "list":
                    ListAllTasks();
                    break;
                case "create":
                    CreateTask(commandArguments);
                    break;
                case "describe":
                    DescribeTask(commandArguments);
                    break;
                default:
                    System.Console.WriteLine("Unkown command");
                    break;
            }
        }

        private static void DescribeTask(IEnumerable<string> commandArguments)
        {
            var taskId = new Guid(commandArguments.First());
            var taskDescription = string.Join(" ", commandArguments.Skip(1));

            var task = aggregateRepository.GetById<Task>(taskId);

            task.Describe(taskDescription);

            aggregateRepository.Save(task, Guid.NewGuid());

            System.Console.WriteLine($"Description for task {task.Id} set");
        }

        private static void CreateTask(IEnumerable<string> commandArguments)
        {
            var taskTitle = commandArguments.First();
            var taskDescription = string.Join(" ", commandArguments.Skip(1));

            var task = new Task(taskTitle);

            if (!string.IsNullOrEmpty(taskDescription))
            {
                task.Describe(taskDescription);
            }

            aggregateRepository.Save(task, Guid.NewGuid());

            System.Console.WriteLine($"Task {task.Id} created");
        }

        static void ListAllTasks()
        {
            //var tasks = materializationStrategy.GetAggregates<Task>();

            using (var connection = container.Resolve<Func<IDbConnection>>()())
            {
                var tasks = connection.Query<TaskReadModel>($"select * from {typeof(TaskReadModel).GetTableName()}");
                
                foreach (var task in tasks)
                {
                    System.Console.WriteLine($"{task.Id}({task.Version}): {task.Title} - {task.Description}");
                }
            }
        }

        public static IStoreEvents CreateMemoryConnection()
        {
            return Wireup.Init()
                .LogToOutputWindow()
                .UsingInMemoryPersistence()
                .InitializeStorageEngine()
                .Build();
        }
    }
}
