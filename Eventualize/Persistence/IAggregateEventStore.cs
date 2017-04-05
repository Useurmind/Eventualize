using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Persistence
{
    public interface IAggregateEventStore : IDisposable
    {
        IEnumerable<IAggregateEvent> GetEvents(AggregateIdentity aggregateIdentity, long start, long end);

        void AppendEvents(AggregateIdentity aggregateIdentity, long expectedAggregateVersion, IEnumerable<IEventData> newAggregateEvents, Guid replayId);
    }
}