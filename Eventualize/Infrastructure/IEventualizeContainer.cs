using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Materialization;
using Eventualize.Persistence;

namespace Eventualize.Infrastructure
{
    public interface IEventualizeContainer
    {
        IAggregateEventStore AggregateEventStore { get; }

        IEventualizeLogger Logger { get; }

        IMaterializationEventPoller MaterializationEventPoller { get; }

        IAggregateRepository AggregateRepository { get; }

        IConstructInstances AggregateFactory { get; }

        IEventConverter EventConverter { get; }

        ISerializer Serializer { get; }

        IEnumerable<IMaterializationStrategy> MaterializationStrategies { get; }

        IEnumerable<IAggregateMaterializationStrategy> AggregateMaterializationStrategies { get; }

        TInterface Resolve<TInterface>();
    }
}
