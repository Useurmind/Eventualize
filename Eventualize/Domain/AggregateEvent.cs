using System;
using System.Linq;

namespace Eventualize.Domain
{
    public class AggregateEvent : Event, IAggregateEvent
    {
        public AggregateEvent(long storeIndex, EventNamespace eventSpace, Guid eventId, EventType eventType, DateTime creationTime, UserId creatorId, IEventData eventData, EventStreamIndex eventStreamIndex, AggregateIdentity aggregateIdentity)
            : base(storeIndex, eventSpace, eventId, eventType, creationTime, creatorId, eventData, eventStreamIndex)
        {
            this.AggregateIdentity = aggregateIdentity;
        }

        public AggregateIdentity AggregateIdentity { get; }
    }
}