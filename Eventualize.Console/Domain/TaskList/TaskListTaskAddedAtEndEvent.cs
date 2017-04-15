using System;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Console.Domain.TaskList
{
    [EventTypeName("TaskListTaskAddedAtEnd")]
    public class TaskListTaskAddedAtEndEvent : IEventData
    {
        public TaskListTaskAddedAtEndEvent(Guid taskId)
        {
            this.TaskId = taskId;
        }

        public Guid TaskId { get; }
    }
}