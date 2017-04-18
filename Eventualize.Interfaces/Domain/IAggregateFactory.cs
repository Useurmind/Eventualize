using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Snapshots;

namespace Eventualize.Interfaces.Domain
{
    public interface IAggregateFactory
    {
        IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, ISnapShot snapshot);

        IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, ISnapShot snapshot, IEnumerable<IEventData> events);
    }
}