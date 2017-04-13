using System.Linq;

using Eventualize.Domain;
using Eventualize.Materialization.Progress;

namespace Eventualize.Materialization.AggregateMaterialization
{
    public interface IAggregateMaterializer
    {
        /// <summary>
        /// Returning null means you want all aggregate events.
        /// </summary>
        ChosenAggregateTypes ChosenAggregateTypes { get; }

        ProgessValue GetProgess { get; }

        void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent);
    }
}