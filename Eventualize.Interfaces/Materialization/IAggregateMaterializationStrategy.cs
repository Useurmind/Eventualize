using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization
{
    public interface IAggregateMaterializationStrategy
    {
        void HandleEvent(IAggregateEvent aggregateEvent);
    }
}