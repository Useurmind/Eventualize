using System;
using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;

namespace Eventualize.Console.Domain.EventualizeTest.Tasks
{
    [BoundedContext("EventualizeTest")]
    [AggregateTypeName("Task")]
    public class Task : StateBackedAggregateBase<TaskState>
    {
        public Task()
        {
        }

        public Task(Guid id) : base(id)
        {
        }

        public Task(string title) : this(Guid.NewGuid())
        {
            this.SetTitleAfterCreate(title);
        }

        public string Title => this.State.Title;

        public string Description => this.State.Description;

        private void SetTitleAfterCreate(string title)
        {
            this.RaiseEvent(new TaskCreatedEvent()
            {
                TaskId = this.Id,
                Title = title
            });
        }

        public void Describe(string description)
        {
            this.RaiseEvent(new TaskDescriptionAddedEvent()
            {
                TaskId = this.Id,
                Description = description
            });
        }

        private void Apply(TaskCreatedEvent @event)
        {
            this.State.Title = @event.Title;
        }

        private void Apply(TaskDescriptionAddedEvent @event)
        {
            this.State.Description = @event.Description;
        }
    }
}
