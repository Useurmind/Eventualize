using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Materialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;
using Eventualize.Persistence.Snapshots;

namespace Eventualize.Infrastructure
{
    public interface IEventualizeContainer
    {
        IAggregateEventStore AggregateEventStore { get; }

        IEventualizeLogger Logger { get; }

        IMaterializationEventPoller MaterializationEventPoller { get; }

        IMaterializationProgessStore MaterializationProgessStore { get; }

        IAggregateRepository AggregateRepository { get; }

        ISnapShotStore SnapShotStore{ get; }

        IConstructInstances AggregateFactory { get; }

        ISnapshotConverter SnapshotConverter { get; }

        IEventConverter EventConverter { get; }

        ISerializer Serializer { get; }

        IEnumerable<IMaterializationStrategy> MaterializationStrategies { get; }

        IEnumerable<IAggregateMaterializationStrategy> AggregateMaterializationStrategies { get; }

        TInterface Resolve<TInterface>();
    }
}
