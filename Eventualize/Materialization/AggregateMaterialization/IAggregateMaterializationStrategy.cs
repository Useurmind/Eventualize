using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization.AggregateMaterialization
{
    public interface IAggregateMaterializationStrategy
    {
        void HandleEvent(IAggregateEvent aggregateEvent);
    }
}