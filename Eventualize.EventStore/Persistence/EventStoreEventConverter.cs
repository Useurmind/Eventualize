using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.Domain.Core;
using Eventualize.Persistence;
using Eventualize.Security;

namespace Eventualize.EventStore.Persistence
{
    public class EventMetaData
    {
        public string CreatorId { get; set; }
    }

    public class EventStoreEventConverter : IEventStoreEventConverter
    {
        private IEventConverter eventConverter;

        private ISerializer serializer;

        public EventStoreEventConverter(IEventConverter eventConverter, ISerializer serializer)
        {
            this.eventConverter = eventConverter;
            this.serializer = serializer;
        }

        public IAggregateEvent GetDomainEvent(AggregateIdentity aggregateIdentity, RecordedEvent recordedEvent)
        {
            var eventData = this.eventConverter.DeserializeEventData(recordedEvent.EventType, recordedEvent.EventId, recordedEvent.Data);
            var metaData = (EventMetaData)this.serializer.Deserialize(typeof(EventMetaData), recordedEvent.Metadata);

            return new AggregateEvent(
                storeIndex: -1,
               eventId: recordedEvent.EventId,
               eventType: recordedEvent.EventType,
               creationTime: recordedEvent.Created,
               creatorId: metaData.CreatorId,
               eventData: eventData,
               aggregateIdentity: aggregateIdentity,
               aggregateIndex: recordedEvent.EventNumber
                );
        }

        public IEvent GetDomainEvent(RecordedEvent recordedEvent)
        {
            var eventData = this.eventConverter.DeserializeEventData(recordedEvent.EventType, recordedEvent.EventId, recordedEvent.Data);
            var metaData = (EventMetaData)this.serializer.Deserialize(typeof(EventMetaData), recordedEvent.Metadata);

            return new Event(
                storeIndex: -1,
               eventId: recordedEvent.EventId,
               eventType: recordedEvent.EventType,
               creationTime: recordedEvent.Created,
               creatorId: metaData.CreatorId,
               eventData: eventData
                );
        }

        public EventData GetEventData(IEventData eventData)
        {
            var eventMetaData = new EventMetaData()
                                {
                                    CreatorId = EventualizeContext.Current.CurrentUser.UserId
                                };

            return new EventData(
                Guid.NewGuid(),
                eventData.GetEventTypeName(),
                true,
                this.eventConverter.SerializeEventData(eventData),
                this.serializer.Serialize(eventMetaData));
        }
    }
}
