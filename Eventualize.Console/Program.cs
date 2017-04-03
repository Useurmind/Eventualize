using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using Autofac;

using EventStore.ClientAPI.Embedded;
using EventStore.Core;

using Eventualize.Autofac.Infrastructure;
using Eventualize.Console.Domain;
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
        private static IRepository repository;

        private static InMemoryMaterialization materializationStrategy;

        static void Main(string[] args)
        {
            using (var container = SetupContainer(true))
            {
                var eventContainer = container.Eventualize();

                eventContainer.Materializer.Run();

                repository = eventContainer.Repository;
                materializationStrategy = eventContainer.MaterializationStrategies.OfType<InMemoryMaterialization>().First();

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

            builder.Eventualize(
                b =>
                {
                    b.SetDefaults(Assembly.GetExecutingAssembly());
                    b.MaterializeInMemory();
                });

            if (forEventStore)
            {
                builder.Eventualize(
                    b =>
                    {
                        b.RegisterSingleInstance(
                             c =>
                             {
                                 var node = EmbeddedVNodeBuilder.AsSingleNode().OnDefaultEndpoints().RunInMemory().Build();
                                 node.StartAndWaitUntilReady();
                                 return node;
                             })
                         .ConnectEventStore(c => EmbeddedEventStoreConnection.Create(c.Resolve<ClusterVNode>()))
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

            var task = repository.GetById<Task>(taskId);

            task.Describe(taskDescription);
            
            repository.Save(task, Guid.NewGuid());

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

            repository.Save(task, Guid.NewGuid());

            System.Console.WriteLine($"Task {task.Id} created");
        }

        static void ListAllTasks()
        {
            var tasks = materializationStrategy.GetAggregates<Task>();

            foreach (var task in tasks)
            {
                System.Console.WriteLine($"{task.Id}: {task.Title} - {task.Description}");
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
