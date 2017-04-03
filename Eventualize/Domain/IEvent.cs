using System;
using System.Linq;

namespace Eventualize.Domain
{
    public interface IEvent
    {
        Guid EventId { get; }

        string EventType { get; }

        DateTime CreationTime { get; }

        object EventData { get; }
    }
}