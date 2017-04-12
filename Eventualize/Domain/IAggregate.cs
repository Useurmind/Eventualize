using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Eventualize.Domain
{
    public interface IAggregate
    {
        Guid Id { get; }

        long CommittedVersion { get; }

        long Version { get; }

        void ApplyEvent(object @event);

        ICollection GetUncommittedEvents();

        void ClearUncommittedEvents();

        IMemento GetSnapshot();

        void ApplySnapshot(IMemento snapshot);
    }
}
