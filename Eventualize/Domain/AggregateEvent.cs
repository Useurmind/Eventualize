using System;
using System.Linq;

namespace Eventualize.Domain
{
    public class AggregateEvent : Event, IAggregateEvent
    {
        public AggregateEvent(long storeIndex, EventNamespace eventSpace, Guid eventId, EventType eventType, DateTime creationTime, UserId creatorId, IEventData eventData, AggregateIdentity aggregateIdentity, long aggregateIndex)
            : base(storeIndex, eventSpace, eventId, eventType, creationTime, creatorId, eventData)
        {
            this.AggregateIdentity = aggregateIdentity;
            this.AggregateIndex = aggregateIndex;
        }

        public AggregateIdentity AggregateIdentity { get; }

        public long AggregateIndex { get; }
    }
}