using System;
using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Domain.Events
{
    public class Event : IEvent
    {
        public Event(long storeIndex, BoundedContextName boundedContextName, Guid eventId, EventTypeName eventTypeName, DateTime creationTime, UserId creatorId, IEventData eventData, EventStreamIndex streamIndex)
        {
            this.StoreIndex = storeIndex;
            this.BoundedContextName = boundedContextName;
            this.EventId = eventId;
            this.EventTypeName = eventTypeName;
            this.CreationTime = creationTime;
            this.CreatorId = creatorId;
            this.EventData = eventData;
            this.StreamIndex = streamIndex;
        }

        public long StoreIndex { get; }

        public BoundedContextName BoundedContextName { get; }

        public Guid EventId { get; }

        public EventTypeName EventTypeName { get; }

        public DateTime CreationTime { get; }

        public UserId CreatorId { get; }

        public IEventData EventData { get; }

        public EventStreamIndex StreamIndex { get; }
    }
}