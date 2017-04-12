using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Console.Domain.TaskList
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