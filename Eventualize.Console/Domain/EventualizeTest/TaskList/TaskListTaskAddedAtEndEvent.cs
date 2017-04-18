using System;
using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Console.Domain.EventualizeTest.TaskList
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