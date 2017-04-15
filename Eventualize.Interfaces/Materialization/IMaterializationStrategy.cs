using System.Linq;

using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Interfaces.Materialization
{
    public interface IMaterializationStrategy
    {
        void HandleEvent(IEvent @event);
    }
}