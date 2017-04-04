using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Persistence
{
    public interface IConstructInstances
    {
        IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, IMemento snapshot);

        IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, IMemento snapshot, IEnumerable<IEventData> events);
    }
}