using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public interface IAggregateMaterializationStrategy
    {
        void HandleEvent(IAggregateEvent aggregateEvent);
    }
}