using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;

namespace Eventualize.Test.TestDomain.SecondContext.MyThirdAggregate
{
    public class MyThirdAggregate : StateBackedAggregateBase<MyThirdAggregateState>
    {
        public void ExecuteCommandForMyThirdEvent()
        {
            this.RaiseEvent(new MyThirdEvent());
        }

        private void Apply(MyThirdEvent @event)
        {

        }
    }
}