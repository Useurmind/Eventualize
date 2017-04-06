using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.Domain.Core;
using Eventualize.Persistence;

namespace Eventualize.EventStore.Persistence
{
    public class EventStoreEventConverter : IEventStoreEventConverter
    {
        private IEventConverter eventConverter;

        public EventStoreEventConverter(IEventConverter eventConverter)
        {
            this.eventConverter = eventConverter;
        }

        public IAggregateEvent GetDomainEvent(AggregateIdentity aggregateIdentity, RecordedEvent recordedEvent)
        {
            var eventData = this.eventConverter.DeserializeEventData(recordedEvent.EventType, recordedEvent.EventId, recordedEvent.Data);

            return new AggregateEvent(
                storeIndex: -1,
               eventId: recordedEvent.EventId,
               eventType: recordedEvent.EventType,
               creationTime: recordedEvent.Created,
               creatorId: null,
               eventData: eventData,
               aggregateIdentity: aggregateIdentity,
               aggregateIndex: recordedEvent.EventNumber
                );
        }

        public IEvent GetDomainEvent(RecordedEvent recordedEvent)
        {
            var eventData = this.eventConverter.DeserializeEventData(recordedEvent.EventType, recordedEvent.EventId, recordedEvent.Data);

            return new Event(
                storeIndex: -1,
               eventId: recordedEvent.EventId,
               eventType: recordedEvent.EventType,
               creationTime: recordedEvent.Created,
               creatorId: null,
               eventData: eventData
                );
        }

        public EventData GetEventData(IEventData eventData)
        {
            return new EventData(
                Guid.NewGuid(),
                eventData.GetEventTypeName(),
                true,
                this.eventConverter.SerializeEventData(eventData),
                null);
        }
    }
}
