using System;
using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Domain.Events
{
    public class AggregateEvent : Event, IAggregateEvent
    {
        public AggregateEvent(long storeIndex, BoundedContext boundedContext, Guid eventId, EventType eventType, DateTime creationTime, UserId creatorId, IEventData eventData, EventStreamIndex eventStreamIndex, AggregateIdentity aggregateIdentity)
            : base(storeIndex, boundedContext, eventId, eventType, creationTime, creatorId, eventData, eventStreamIndex)
        {
            this.AggregateIdentity = aggregateIdentity;
        }

        public AggregateIdentity AggregateIdentity { get; }
    }
}