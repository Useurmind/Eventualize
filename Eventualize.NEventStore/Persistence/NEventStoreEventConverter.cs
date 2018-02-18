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
        public static IAggregateEvent CreateAggregateEvent(AggregateIdentity aggregateIdentity, Guid commitId, EventMessage eventMessage, int index)
        {
            // TODO FIX this
           // return new AggregateEvent(
           //     storeIndex: 0,
           //     boundedContextName: aggregateIdentity.BoundedContextName,
           //     eventId: Guid.Empty, 
           //     eventTypeName: ((IEventData)eventMessage.Body).GetEventTypeName(),
           //     creationTime: DateTime.MinValue, 
           //     creatorId: new UserId(), 
           //     eventData: (IEventData)eventMessage.Body,
           //     aggregateIdentity: aggregateIdentity,
           //     eventStreamIndex: new EventStreamIndex(index)
           // );

           // var genericEventType = typeof(AggregateEvent<>).MakeGenericType(eventData.GetType());

           // return (IAggregateEvent)Activator.CreateInstance(
           //    genericEventType,
           //    storeIndex, // storeIndex
           //    aggregateIdentity.BoundedContextName, // boundedContextName
           //    recordedEvent.EventId, // eventId
           //    eventTypeName, // eventTypeName
           //    recordedEvent.Created, // creationTime
           //    new UserId(metaData.CreatorId), // creatorId
           //    eventData, // eventData
           //    aggregateIdentity, // aggregateIdentity 
           //    new EventStreamIndex(recordedEvent.EventNumber) // eventStreamIndex
           //);

            throw new NotImplementedException();
        }
    }
}