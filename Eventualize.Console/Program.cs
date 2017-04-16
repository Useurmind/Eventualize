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
using EventStore.ClientAPI.Exceptions;
using EventStore.Core;

using Eventualize.Autofac.Infrastructure;
using Eventualize.Console.Domain;
using Eventualize.Console.Domain.TaskList;
using Eventualize.Console.ReadModel;
using Eventualize.Dapper.Materialization;
using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.EventStore.Infrastructure;
using Eventualize.Infrastructure;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Persistence;
using Eventualize.Materialization;
using Eventualize.Materialization.AggregateMaterialization;
using Eventualize.NEventStore.Infrastructure;
using Eventualize.NEventStore.Materialization;
using Eventualize.NEventStore.Persistence;
using Eventualize.Persistence;
using Eventualize.Security;

using NEventStore;

namespace Eventualize.Console
{
    class Program
    {
        private static IAggregateRepository aggregateRepository;

        private static InMemoryMaterialization materializationStrategy;

        private static IContainer container;

        private static string userName;

        static void Main(string[] args)
        {
            userName = "Jochen";
            EventualizeContext.Init(new UserId(userName), new BoundedContext("EventualizeTest"));

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
                    new SimpleSingleAggregateMaterializer<Task, TaskReadModel>(c.Resolve<Func<IDbConnection>>())).As<IAggregateMaterializer>();

            builder.Eventualize(
                b =>
                {
                    b.StoreMaterializationProgessInFileSystem();
                    b.SetDefaults(Assembly.GetExecutingAssembly());
                    b.DeriveIdentitiesFromAttributes();
                    b.MaterializePerAggregate();
                    b.MaterializeSnapShots<Task>();
                    b.MaterializeSnapShots<TaskList>();
                });

            if (forEventStore)
            {
                builder.Eventualize(
                    b =>
                    {
                        //b.RegisterSingleInstance(
                        //     c =>
                        //     {
                        //         var node = EmbeddedVNodeBuilder.AsSingleNode()
                        //             .RunInMemory()
                        //             .OnDefaultEndpoints()
                        //             .Build();

                        //         node.Start();
                        //         return node;
                        //     })
                        //.ConnectEventStore(c => EmbeddedEventStoreConnection.Create(c.Resolve<ClusterVNode>()))
                        b.ConnectEventStore(new Uri(@"tcp://admin:changeit@127.0.0.1:1113"), ConnectionSettings.Default)
                        .StoreAggregatesInEventStore()
                        .StoreSnapShotsInEventStore()
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

            if (command.StartsWith("list all"))
            {
                ListAllTasks();
            }
            else if(command.StartsWith("list"))
            {
                ListTasksInList(commandParts.Skip(1));
            }
            else if (command.StartsWith("create list"))
            {
                CreateTaskList(commandParts.Skip(2));
            }
            else if (command.StartsWith("add task to list"))
            {
                AddTaskToList(commandParts.Skip(4));
            }
            else if (command.StartsWith("create task"))
            {
                CreateTask(commandParts.Skip(2));
            }
            else if (command.StartsWith("describe task"))
            {
                DescribeTask(commandParts.Skip(2));
            }
            else
            {
                System.Console.WriteLine("Unkown command");
            }
        }

        private static void ListTasksInList(IEnumerable<string> commandArguments)
        {
            var listId= Guid.Parse(commandArguments.First());


        }

        private static void AddTaskToList(IEnumerable<string> commandArguments)
        {
            var listId = new Guid(commandArguments.First());
            var taskId = new Guid(commandArguments.Skip(1).First());

            var taskList = aggregateRepository.GetById<TaskList>(listId);
            var task = aggregateRepository.GetById<Task>(taskId);

            taskList.AddTaskAtEnd(taskId);
        }

        private static void CreateTaskList(IEnumerable<string> commandArguments)
        {
            var listName = commandArguments.First();
            var taskList = new TaskList(listName);
            aggregateRepository.Save(taskList, Guid.NewGuid());
            System.Console.WriteLine($"Created task list {taskList.Id}");
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
