using System;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public interface IMaterializationStrategy
    {
        void HandleEvent(Func<IAggregate> createAggregate, Guid aggregateId, object @event);
    }
}