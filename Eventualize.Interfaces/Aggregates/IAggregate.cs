using System;
using System.Collections;
using System.Linq;

using Eventualize.Interfaces.Snapshots;

namespace Eventualize.Interfaces.Aggregates
{
    public interface IAggregate
    {
        Guid Id { get; }

        long CommittedVersion { get; }

        long Version { get; }

        void ApplyEvent(object @event);

        ICollection GetUncommittedEvents();

        void ClearUncommittedEvents();

        ISnapShot GetSnapshot();

        void ApplySnapshot(ISnapShot snapshot);
    }
}
