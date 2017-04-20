using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Progress;
using Eventualize.Interfaces.Persistence;
using Eventualize.Interfaces.Snapshots;

namespace Eventualize.Interfaces.Infrastructure
{
    public interface IEventualizeContainer
    {
        IDomainMetaModel DomainMetaModel { get; }

        IDomainIdentityProvider DomainIdentityProvider { get; }

        IAggregateEventStore AggregateEventStore { get; }

        IEventualizeLogger Logger { get; }

        IMaterializationEventPoller MaterializationEventPoller { get; }

        IMaterializationProgessStore MaterializationProgessStore { get; }

        IAggregateRepository AggregateRepository { get; }

        ISnapShotStore SnapShotStore{ get; }

        IAggregateFactory AggregateFactory { get; }

        ISnapshotConverter SnapshotConverter { get; }

        IEventConverter EventConverter { get; }

        ISerializer Serializer { get; }

        IEnumerable<IMaterializationStrategy> MaterializationStrategies { get; }

        IEnumerable<IAggregateMaterializationStrategy> AggregateMaterializationStrategies { get; }

        TInterface Resolve<TInterface>();
    }
}
