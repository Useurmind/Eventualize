using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Aggregates
{
    public interface IEvent
    {
        /// <summary>
        /// Index of the event in the complete stream of events in the storage.
        /// </summary>
        long StoreIndex { get; }

        BoundedContext BoundedContext { get; }

        Guid EventId { get; }

        EventType EventType { get; }

        DateTime CreationTime { get; }

        UserId CreatorId { get; }

        IEventData EventData { get; }
    }
}