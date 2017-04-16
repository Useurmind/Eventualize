using System;
using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Domain.Events
{
    public class Event : IEvent
    {
        public Event(long storeIndex, BoundedContext boundedContext, Guid eventId, EventType eventType, DateTime creationTime, UserId creatorId, IEventData eventData, EventStreamIndex streamIndex)
        {
            this.StoreIndex = storeIndex;
            this.BoundedContext = boundedContext;
            this.EventId = eventId;
            this.EventType = eventType;
            this.CreationTime = creationTime;
            this.CreatorId = creatorId;
            this.EventData = eventData;
            this.StreamIndex = streamIndex;
        }

        public long StoreIndex { get; }

        public BoundedContext BoundedContext { get; }

        public Guid EventId { get; }

        public EventType EventType { get; }

        public DateTime CreationTime { get; }

        public UserId CreatorId { get; }

        public IEventData EventData { get; }

        public EventStreamIndex StreamIndex { get; }
    }
}