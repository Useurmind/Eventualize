using System;
using System.Linq;

namespace Eventualize.Domain
{
    public class Event : IEvent
    {
        public Event(long storeIndex, EventNamespace eventSpace, Guid eventId, EventType eventType, DateTime creationTime, UserId creatorId, IEventData eventData)
        {
            this.StoreIndex = storeIndex;
            this.EventSpace = eventSpace;
            this.EventId = eventId;
            this.EventType = eventType;
            this.CreationTime = creationTime;
            this.CreatorId = creatorId;
            this.EventData = eventData;
        }

        public long StoreIndex { get; }

        public EventNamespace EventSpace { get; }

        public Guid EventId { get; }

        public EventType EventType { get; }

        public DateTime CreationTime { get; }

        public UserId CreatorId { get; }

        public IEventData EventData { get; }
    }
}