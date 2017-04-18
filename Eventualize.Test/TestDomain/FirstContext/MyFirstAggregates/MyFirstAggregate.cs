using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;

namespace Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates
{
    [BoundedContext(DomainNames.FirstContextName)]
    [AggregateTypeName("MyFirstAggregateWoot")]
    public class MyFirstAggregate : StateBackedAggregateBase<MyFirstAggregateState>
    {
        public void ExecuteCommandForMyFirstEvent()
        {
            this.RaiseEvent(new MyFirstEvent());
        }

        private void Apply(MyFirstEvent @event)
        {

        }
    }
}