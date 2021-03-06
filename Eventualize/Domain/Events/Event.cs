using System;
using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Domain.Events
{
    public class Event<TData> : IEvent<TData>
        where TData : IEventData
    {
        public Event(long storeIndex, BoundedContextName boundedContextName, Guid eventId, EventTypeName eventTypeName, DateTime creationTime, UserId creatorId, TData eventData, EventStreamIndex streamIndex)
        {
            this.StoreIndex = storeIndex;
            this.BoundedContextName = boundedContextName;
            this.EventId = eventId;
            this.EventTypeName = eventTypeName;
            this.CreationTime = creationTime;
            this.CreatorId = creatorId;
            this.EventData = eventData;
            this.EventStreamIndex = streamIndex;
        }

        public long StoreIndex { get; }

        public BoundedContextName BoundedContextName { get; }

        public Guid EventId { get; }

        public EventTypeName EventTypeName { get; }

        public DateTime CreationTime { get; }

        public UserId CreatorId { get; }

        IEventData IEvent.EventData
        {
            get
            {
                return this.EventData;
            }
        }

        public TData EventData { get; }

        public EventStreamIndex EventStreamIndex { get; }
    }
}