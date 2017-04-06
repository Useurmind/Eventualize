using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Console.Domain.TaskList
{
    public class TaskList : AggregateBase
    {
        public TaskList(string name)
            : base(null)
        {
            this.RaiseEvent(new TaskListCreatedEvent(name));
        }

        public string Name { get; private set; }

        public IEnumerable<Guid> Tasks { get; private set; }

        public void AddTaskAtEnd(Guid taskId)
        {
            this.RaiseEvent(new TaskListTaskAddedAtEndEvent(taskId));
        }

        private void Apply(TaskListCreatedEvent @event)
        {
            this.Name = @event.Name;
            this.Tasks = Enumerable.Empty<Guid>();
        }

        private void Apply(TaskListTaskAddedAtEndEvent @event)
        {
            this.Tasks = this.Tasks.Union(new[] { @event.TaskId });
        }
    }

    [EventTypeName("TaskListCreated")]
    public class TaskListCreatedEvent : IEventData
    {
        public TaskListCreatedEvent(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }

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