using System.Linq;

using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Interfaces.Materialization
{
    public interface IAggregateMaterializationStrategy
    {
        void HandleEvent(IAggregateEvent aggregateEvent);
    }
}