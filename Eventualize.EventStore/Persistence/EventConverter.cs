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
    public interface IEventStoreEventConverter
    {
        IEventData GetDomainEvent(RecordedEvent recordedEvent);

        EventData GetEventData(IEventData eventData);
    }

    public class EventStoreEventConverter : IEventStoreEventConverter
    {
        private IEventConverter eventConverter;

        public EventStoreEventConverter(IEventConverter eventConverter)
        {
            this.eventConverter = eventConverter;
        }

        public IEventData GetDomainEvent(RecordedEvent recordedEvent)
        {
            return this.eventConverter.DeserializeEventData(recordedEvent.EventType, recordedEvent.EventId, recordedEvent.Data);
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
