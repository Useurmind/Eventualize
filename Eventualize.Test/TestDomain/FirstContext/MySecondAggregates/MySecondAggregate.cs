using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;

namespace Eventualize.Test.TestDomain.FirstContext.MySecondAggregates
{
    [BoundedContext(DomainNames.FirstContextName)]
    public class MySecondAggregate : StateBackedAggregateBase<MySecondAggregateState>
    {
        public void ExecuteCommandForMySecondEvent()
        {
            this.RaiseEvent(new MySecondEvent());
        }

        private void Apply(MySecondEvent @event)
        {

        }
    }
}