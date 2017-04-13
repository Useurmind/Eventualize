using System;
using System.Linq;

namespace Eventualize.Domain
{
    public struct EventStreamIndex
    {
        public EventStreamIndex(long value)
        {
            this.Value = value;
        }

        public long Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Event : IEvent
    {
        public Event(long storeIndex, EventNamespace eventSpace, Guid eventId, EventType eventType, DateTime creationTime, UserId creatorId, IEventData eventData, EventStreamIndex streamIndex)
        {
            this.StoreIndex = storeIndex;
            this.EventSpace = eventSpace;
            this.EventId = eventId;
            this.EventType = eventType;
            this.CreationTime = creationTime;
            this.CreatorId = creatorId;
            this.EventData = eventData;
            this.StreamIndex = streamIndex;
        }

        public long StoreIndex { get; }

        public EventNamespace EventSpace { get; }

        public Guid EventId { get; }

        public EventType EventType { get; }

        public DateTime CreationTime { get; }

        public UserId CreatorId { get; }

        public IEventData EventData { get; }

        public EventStreamIndex StreamIndex { get; }
    }
}