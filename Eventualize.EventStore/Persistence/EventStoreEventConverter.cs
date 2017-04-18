using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Infrastructure;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Infrastructure;
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
                boundedContextName: aggregateIdentity.BoundedContextName,
               eventId: recordedEvent.EventId,
               eventTypeName: new EventTypeName(recordedEvent.EventType),
               creationTime: recordedEvent.Created,
               creatorId: new UserId(metaData.CreatorId),
               eventData: eventData,
               aggregateIdentity: aggregateIdentity,
               eventStreamIndex: new EventStreamIndex(recordedEvent.EventNumber)
                );
        }

        public IEvent GetDomainEvent(RecordedEvent recordedEvent, BoundedContextName boundedContextName)
        {
            var eventData = this.eventConverter.DeserializeEventData(recordedEvent.EventType, recordedEvent.EventId, recordedEvent.Data);
            var metaData = (EventMetaData)this.serializer.Deserialize(typeof(EventMetaData), recordedEvent.Metadata);

            return new Event(
                storeIndex: -1,
                boundedContextName: boundedContextName,
               eventId: recordedEvent.EventId,
               eventTypeName: new EventTypeName(recordedEvent.EventType), 
               creationTime: recordedEvent.Created,
               creatorId: new UserId(metaData.CreatorId),
               eventData: eventData,
               streamIndex: new EventStreamIndex(recordedEvent.EventNumber)
                );
        }

        public EventData GetEventData(IEventData eventData)
        {
            var eventMetaData = new EventMetaData()
            {
                CreatorId = EventualizeContext.Current.CurrentUser.UserId.Value
            };

            return new EventData(
                Guid.NewGuid(),
                eventData.GetEventTypeName().Value,
                true,
                this.eventConverter.SerializeEventData(eventData),
                this.serializer.Serialize(eventMetaData));
        }
    }
}
