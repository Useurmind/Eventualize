using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Persistence
{
    public interface IAggregateEventStore : IDisposable
    {
        IEnumerable<IAggregateEvent> GetEvents(AggregateIdentity aggregateIdentity, AggregateVersion start, AggregateVersion end);

        void AppendEvents(AggregateIdentity aggregateIdentity, AggregateVersion expectedAggregateVersion, IEnumerable<IEventData> newAggregateEvents, Guid replayId);
    }
}