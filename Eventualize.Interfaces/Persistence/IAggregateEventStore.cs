using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Persistence
{
    public interface IAggregateEventStore : IDisposable
    {
        IEnumerable<IAggregateEvent> GetEvents(AggregateIdentity aggregateIdentity, long start, long end);

        void AppendEvents(AggregateIdentity aggregateIdentity, long expectedAggregateVersion, IEnumerable<IEventData> newAggregateEvents, Guid replayId);
    }
}