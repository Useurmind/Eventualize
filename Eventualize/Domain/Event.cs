using System;
using System.Linq;

namespace Eventualize.Domain
{
    public class Event : IEvent
    {
        public Event(long storeIndex, Guid eventId, string eventType, DateTime creationTime, string creatorId, IEventData eventData)
        {
            this.StoreIndex = storeIndex;
            this.EventId = eventId;
            this.EventType = eventType;
            this.CreationTime = creationTime;
            this.CreatorId = creatorId;
            this.EventData = eventData;
        }

        public long StoreIndex { get; }

        public Guid EventId { get; }

        public string EventType { get; }

        public DateTime CreationTime { get; }

        public string CreatorId { get; }

        public IEventData EventData { get; }
    }
}