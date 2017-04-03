using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Eventualize.Domain
{
    public interface IAggregate
    {
        Guid Id { get; }

        int CommittedVersion { get; }

        int Version { get; }

        void ApplyEvent(object @event);

        ICollection GetUncommittedEvents();

        void ClearUncommittedEvents();

        IMemento GetSnapshot();
    }
}
