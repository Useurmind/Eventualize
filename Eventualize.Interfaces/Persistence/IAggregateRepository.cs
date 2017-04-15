using System;
using System.Linq;

using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Persistence
{
    public interface IAggregateRepository : IDisposable
    {
        TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;

        TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;

        IAggregate GetById(AggregateIdentity aggregateIdentity, int version);

        IAggregate GetById(AggregateIdentity aggregateIdentity);

        void Save(IAggregate aggregate, Guid replayGuid);

        IAggregate Refresh(IAggregate aggregate);
    }
}
