using System;

using Eventualize.Domain;
using Eventualize.Domain.Core;

using NEventStore;

namespace Eventualize.NEventStore.Persistence
{
    public class NEventStoreEventConverter
    {
        public static AggregateEvent CreateAggregateEvent(AggregateIdentity aggregateIdentity, Guid commitId, EventMessage eventMessage, int index)
        {
            return new AggregateEvent(
                storeIndex: 0,
                eventId: Guid.Empty, 
                eventType: ((IEventData)eventMessage.Body).GetEventTypeName(),
                creationTime: DateTime.MinValue, 
                creatorId: null,
                eventData: (IEventData)eventMessage.Body,
                aggregateIdentity: aggregateIdentity,
                aggregateIndex: index
            );
        }
    }
}