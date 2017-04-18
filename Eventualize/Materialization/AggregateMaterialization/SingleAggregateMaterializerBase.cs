using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;

namespace Eventualize.Materialization.AggregateMaterialization
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