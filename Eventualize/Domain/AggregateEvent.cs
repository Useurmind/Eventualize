using System;
using System.Linq;

namespace Eventualize.Domain
{
    public class AggregateEvent : Event, IAggregateEvent
    {
        public AggregateEvent(long storeIndex, Guid eventId, string eventType, DateTime creationTime, string creatorId, IEventData eventData, AggregateIdentity aggregateIdentity, long aggregateIndex)
            : base(storeIndex, eventId, eventType, creationTime, creatorId, eventData)
        {
            this.AggregateIdentity = aggregateIdentity;
            this.AggregateIndex = aggregateIndex;
        }

        public AggregateIdentity AggregateIdentity { get; }

        public long AggregateIndex { get; }
    }
}