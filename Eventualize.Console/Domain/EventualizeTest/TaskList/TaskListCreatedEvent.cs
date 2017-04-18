using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Console.Domain.EventualizeTest.TaskList
{
    [EventTypeName("TaskListCreated")]
    public class TaskListCreatedEvent : IEventData
    {
        public TaskListCreatedEvent(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}