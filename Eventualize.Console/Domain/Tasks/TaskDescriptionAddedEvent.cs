using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Console.Domain
{
    [EventTypeName("TaskDescribed")]
    public class TaskDescriptionAddedEvent : IEventData
    {
        public string Description { get; set; }
    }
}