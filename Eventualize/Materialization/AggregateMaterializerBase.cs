using System;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public abstract class AggregateMaterializerBase<TAggregate> : IAggregateMaterializer
        where TAggregate : class, IAggregate
    {
        public Type AggregateType
        {
            get
            {
                return typeof(TAggregate);
            }
        }

        public void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent)
        {
            this.HandleAggregateEvent((TAggregate)aggregate, materializationEvent);
        }

        protected abstract void HandleAggregateEvent(TAggregate aggregate, IAggregateEvent materializationEvent);
    }
}