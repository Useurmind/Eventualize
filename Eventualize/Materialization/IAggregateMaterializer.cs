using System;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public interface IAggregateMaterializer
    {
        Type AggregateType { get; }

        void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent);
    }
}