using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.Snapshots;

namespace Eventualize.Interfaces.Domain
{
    public interface IAggregate
    {
        Guid Id { get; }

        long CommittedVersion { get; }

        long Version { get; }

        void ApplyEvent(object @event);

        ICollection<IEventData> GetUncommittedEvents();

        void ClearUncommittedEvents();

        ISnapShot GetSnapshot();

        void ApplySnapshot(ISnapShot snapshot);
    }
}
