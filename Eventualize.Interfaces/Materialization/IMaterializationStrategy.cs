using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization
{
    public interface IMaterializationStrategy
    {
        void HandleEvent(IEvent @event);
    }
}