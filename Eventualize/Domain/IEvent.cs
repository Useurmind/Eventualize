using System;
using System.Linq;

namespace Eventualize.Domain
{
    public interface IEvent
    {
        /// <summary>
        /// Index of the event in the complete stream of events in the storage.
        /// </summary>
        long StoreIndex { get; }

        EventNamespace EventSpace { get; }

        Guid EventId { get; }

        EventType EventType { get; }

        DateTime CreationTime { get; }

        UserId CreatorId { get; }

        IEventData EventData { get; }
    }
}