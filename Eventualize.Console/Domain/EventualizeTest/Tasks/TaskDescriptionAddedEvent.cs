using System;
using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Console.Domain.EventualizeTest.Tasks
{
    [EventTypeName("TaskDescribed")]
    public class TaskDescriptionAddedEvent : IEventData
    {
        public Guid TaskId { get; set; }
        public string Description { get; set; }
    }
}