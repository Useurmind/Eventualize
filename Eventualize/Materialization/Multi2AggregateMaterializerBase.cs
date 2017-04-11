using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public abstract class Multi2AggregateMaterializerBase<TAggregate1, TAggregate2> : IAggregateMaterializer
        where TAggregate1 : class, IAggregate
        where TAggregate2 : class, IAggregate
    {
        public IEnumerable<Type> AggregateTypes
        {
            get
            {
                return new[] { typeof(TAggregate1), typeof(TAggregate2) };
            }
        }

        public void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent)
        {
            if (aggregate is TAggregate1)
            {
                this.HandleAggregateEvent((TAggregate1)aggregate, materializationEvent);
            }
            else
            {
                this.HandleAggregateEvent((TAggregate2)aggregate, materializationEvent);
            }
        }

        protected abstract void HandleAggregateEvent(TAggregate1 aggregate, IAggregateEvent materializationEvent);

        protected abstract void HandleAggregateEvent(TAggregate2 aggregate, IAggregateEvent materializationEvent);
    }
}