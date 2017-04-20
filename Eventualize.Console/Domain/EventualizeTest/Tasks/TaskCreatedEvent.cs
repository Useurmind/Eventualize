using System;
using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Console.Domain.EventualizeTest.Tasks
{
    [EventTypeName("TaskCreated")]
    public class TaskCreatedEvent : IEventData
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
    }
}