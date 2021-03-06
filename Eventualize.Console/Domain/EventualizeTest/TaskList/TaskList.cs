﻿using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Console.Domain.EventualizeTest.TaskList
{
    [BoundedContext("EventualizeTest")]
    [AggregateTypeName("TaskList")]
    public class TaskList : StateBackedAggregateBase<TaskListState>
    {
        public TaskList()
        {
        }

        public TaskList(Guid id)
            : base(id)
        {

        }

        public TaskList(string name)
        {
            this.RaiseEvent(new TaskListCreatedEvent(name));
        }

        public string Name => this.State.Name;

        public IEnumerable<Guid> Tasks => this.State.Tasks;

        public void AddTaskAtEnd(Guid taskId)
        {
            this.RaiseEvent(new TaskListTaskAddedAtEndEvent(taskId));
        }

        private void Apply(TaskListCreatedEvent @event)
        {
            this.State.Name = @event.Name;
            this.State.Tasks = Enumerable.Empty<Guid>();
        }

        private void Apply(TaskListTaskAddedAtEndEvent @event)
        {
            this.State.Tasks = this.Tasks.Union(new[] { @event.TaskId });
        }
    }
}