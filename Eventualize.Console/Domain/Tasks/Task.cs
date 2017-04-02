﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Console.Domain
{
    [AggregateTypeName("Task")]
    public class Task : AggregateBase
    {
        public Task(Guid id)
        {
            this.Id = id;
        }

        public Task(string title): this(Guid.NewGuid())
        {
            this.SetTitleAfterCreate(title);
        }

        public string Title { get; private set; }

        public string Description { get; private set; }

        private void SetTitleAfterCreate(string title)
        {
            this.RaiseEvent(new TaskCreatedEvent()
                                {
                                    Title = title
                                });
        }

        public void Describe(string description)
        {
            this.RaiseEvent(new TaskDescriptionAddedEvent()
                                {
                                    Description = description
                                });
        }

        private void Apply(TaskCreatedEvent @event)
        {
            this.Title = @event.Title;
        }

        private void Apply(TaskDescriptionAddedEvent @event)
        {
            this.Description = @event.Description;
        }
    }
}