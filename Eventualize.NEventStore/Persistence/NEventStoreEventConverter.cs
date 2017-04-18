using System;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

using NEventStore;

namespace Eventualize.NEventStore.Persistence
{
    public class NEventStoreEventConverter
    {
        public static AggregateEvent CreateAggregateEvent(AggregateIdentity aggregateIdentity, Guid commitId, EventMessage eventMessage, int index)
        {
            return new AggregateEvent(
                storeIndex: 0,
                boundedContextName: aggregateIdentity.BoundedContextName,
                eventId: Guid.Empty, 
                eventTypeName: ((IEventData)eventMessage.Body).GetEventTypeName(),
                creationTime: DateTime.MinValue, 
                creatorId: new UserId(), 
                eventData: (IEventData)eventMessage.Body,
                aggregateIdentity: aggregateIdentity,
                eventStreamIndex: new EventStreamIndex(index)
            );
        }
    }
}