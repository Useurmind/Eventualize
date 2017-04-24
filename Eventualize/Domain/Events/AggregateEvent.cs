using System;
using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Domain.Events
{
    public class AggregateEvent<TData> : Event<TData>, IAggregateEvent<TData>
        where TData : IEventData
    {
        public AggregateEvent(long storeIndex, BoundedContextName boundedContextName, Guid eventId, EventTypeName eventTypeName, DateTime creationTime, UserId creatorId, TData eventData, EventStreamIndex eventStreamIndex, AggregateIdentity aggregateIdentity)
            : base(storeIndex, boundedContextName, eventId, eventTypeName, creationTime, creatorId, eventData, eventStreamIndex)
        {
            this.AggregateIdentity = aggregateIdentity;
        }

        public AggregateIdentity AggregateIdentity { get; }
    }
}