using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public abstract class SingleAggregateMaterializerBase<TAggregate> : IAggregateMaterializer
        where TAggregate : class, IAggregate
    {
        public ChosenAggregateTypes ChosenAggregateTypes
        {
            get
            {
                return new ChosenAggregateTypes(new [] { typeof(TAggregate) });
            }
        }

        public void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent)
        {
            this.HandleAggregateEvent((TAggregate)aggregate, materializationEvent);
        }

        protected abstract void HandleAggregateEvent(TAggregate aggregate, IAggregateEvent materializationEvent);
    }
}