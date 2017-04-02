using System;
using System.Linq;

namespace Eventualize.Domain
{
    public interface IEvent
    {
        string AggregateType { get; }

        string EventType { get; }

        DateTime Time { get; }

        object EventData { get; }
    }
}