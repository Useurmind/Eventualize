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

        Guid EventId { get; }

        string EventType { get; }

        DateTime CreationTime { get; }

        IEventData EventData { get; }
    }
}