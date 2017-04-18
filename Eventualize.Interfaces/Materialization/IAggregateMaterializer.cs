using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization
{
    public interface IAggregateMaterializer
    {
        /// <summary>
        /// Returning null means you want all aggregate events.
        /// </summary>
        ChosenAggregateTypes ChosenAggregateTypes { get; }

        // TODO: we will tend to this later.
        //ProgessValue GetProgess { get; }

        void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent);
    }
}