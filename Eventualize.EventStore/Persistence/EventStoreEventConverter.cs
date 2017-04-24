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
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
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

        public IAggregateEvent GetDomainEvent(AggregateIdentity aggregateIdentity, RecordedEvent recordedEvent, long storeIndex)
        {
            var eventTypeName = new EventTypeName(recordedEvent.EventType);
            var eventData = this.eventConverter.DeserializeEventData(aggregateIdentity.BoundedContextName, eventTypeName, recordedEvent.EventId, recordedEvent.Data);
            var metaData = (EventMetaData)this.serializer.Deserialize(typeof(EventMetaData), recordedEvent.Metadata);

            var genericEventType = typeof(AggregateEvent<>).MakeGenericType(eventData.GetType());

            return (IAggregateEvent)Activator.CreateInstance(
                genericEventType,
                storeIndex, // storeIndex
                aggregateIdentity.BoundedContextName, // boundedContextName
                recordedEvent.EventId, // eventId
                eventTypeName, // eventTypeName
                recordedEvent.Created, // creationTime
                new UserId(metaData.CreatorId), // creatorId
                eventData, // eventData
                aggregateIdentity, // aggregateIdentity 
                new EventStreamIndex(recordedEvent.EventNumber) // eventStreamIndex
            );
        }

        public IEvent GetDomainEvent(RecordedEvent recordedEvent, BoundedContextName boundedContextName)
        {
            var eventTypeName = new EventTypeName(recordedEvent.EventType);
            var eventData = this.eventConverter.DeserializeEventData(boundedContextName, eventTypeName, recordedEvent.EventId, recordedEvent.Data);
            var metaData = (EventMetaData)this.serializer.Deserialize(typeof(EventMetaData), recordedEvent.Metadata);

            var genericEventType = typeof(Event<>).MakeGenericType(eventData.GetType());

            return (IAggregateEvent)Activator.CreateInstance(
                genericEventType,
                -1, // storeIndex
                boundedContextName, // boundedContextName
                recordedEvent.EventId, // eventId
                eventTypeName, // eventTypeName
                recordedEvent.Created, // creationTime
                new UserId(metaData.CreatorId), // creatorId
                eventData, // eventData
                new EventStreamIndex(recordedEvent.EventNumber) // eventStreamIndex
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
