using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Console.Domain
{
    [EventTypeName("TaskCreated")]
    public class TaskCreatedEvent : IEventData
    {
        public string Title { get; set; }
    }
}