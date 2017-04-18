using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Persistence
{
    public interface IAggregateRepository : IDisposable
    {
        TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;

        TAggregate GetById<TAggregate>(Guid id, AggregateVersion version) where TAggregate : class, IAggregate;

        IAggregate GetById(AggregateIdentity aggregateIdentity, AggregateVersion version);

        IAggregate GetById(AggregateIdentity aggregateIdentity);

        void Save(IAggregate aggregate, Guid replayGuid);

        IAggregate Refresh(IAggregate aggregate);
    }
}
