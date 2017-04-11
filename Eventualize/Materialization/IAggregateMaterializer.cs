using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public interface IAggregateMaterializer
    {
        IEnumerable<Type> AggregateTypes { get; }

        void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent);
    }
}