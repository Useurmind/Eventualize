using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using Eventualize.Console.Domain;
using Eventualize.Domain;
using Eventualize.Domain.Core;
using Eventualize.Materialization;
using Eventualize.Persistence;
using Eventualize.Persistence.EventStore;

using NEventStore;

namespace Eventualize.Console
{
    class Program
    {
        private static IRepository repository;

        private static InMemoryMaterialization materializationStrategy;

        private static AggregateFactory aggregateFactory;

        static void Main(string[] args)
        {
            materializationStrategy = new InMemoryMaterialization();
            aggregateFactory = new AggregateFactory();
            var eventStore = CreateMemoryConnection();
            repository = new EventStoreRepository(eventStore, aggregateFactory, new ConflictDetector());
            var materializer = new EventStoreMaterializer(aggregateFactory, eventStore, materializationStrategy, Assembly.GetExecutingAssembly());
            materializer.Run();

            while (true)
            {
                System.Console.WriteLine("Enter a command:");

                var command = System.Console.ReadLine();

                HandleCommand(command);
            }
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

            var task = aggregateFactory.NewAggregate<Task>();
            
            task.SetTitleAfterCreate(taskTitle);

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
