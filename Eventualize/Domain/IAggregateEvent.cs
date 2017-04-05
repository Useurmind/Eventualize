using System.Linq;

namespace Eventualize.Domain
{
    public interface IAggregateEvent : IEvent
    {
        AggregateIdentity AggregateIdentity { get; }

        /// <summary>
        /// Index of the event in the stream of events for this specific aggregate.
        /// </summary>
        long AggregateIndex { get; }
    }
}