using System.Linq;

using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.Materialization;

namespace Eventualize.Materialization.AggregateMaterialization
{
    public abstract class Multi4AggregateMaterializerBase<TAggregate1, TAggregate2, TAggregate3, TAggregate4> : IAggregateMaterializer
        where TAggregate1 : class, IAggregate
        where TAggregate2 : class, IAggregate
        where TAggregate3 : class, IAggregate
        where TAggregate4 : class, IAggregate
    {
        public ChosenAggregateTypes ChosenAggregateTypes
        {
            get
            {
                return new ChosenAggregateTypes(new[] { typeof(TAggregate1), typeof(TAggregate2), typeof(TAggregate3), typeof(TAggregate4) });
            }
        }

        public void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent)
        {
            if (aggregate is TAggregate1)
            {
                this.HandleAggregateEvent((TAggregate1)aggregate, materializationEvent);
            }
            else if (aggregate is TAggregate2)
            {
                this.HandleAggregateEvent((TAggregate2)aggregate, materializationEvent);
            }
            else if (aggregate is TAggregate3)
            {
                this.HandleAggregateEvent((TAggregate3)aggregate, materializationEvent);
            }
            else
            {
                this.HandleAggregateEvent((TAggregate4)aggregate, materializationEvent);
            }
        }

        protected abstract void HandleAggregateEvent(TAggregate1 aggregate, IAggregateEvent materializationEvent);

        protected abstract void HandleAggregateEvent(TAggregate2 aggregate, IAggregateEvent materializationEvent);

        protected abstract void HandleAggregateEvent(TAggregate3 aggregate, IAggregateEvent materializationEvent);

        protected abstract void HandleAggregateEvent(TAggregate4 aggregate, IAggregateEvent materializationEvent);
    }
}