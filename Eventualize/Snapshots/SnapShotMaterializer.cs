using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Snapshots;
using Eventualize.Materialization.AggregateMaterialization;

namespace Eventualize.Snapshots
{
    public class SnapShotMaterializer : IAggregateMaterializer
    {
        private ISnapShotStore snapShotStore;

        private BoundedContext boundedContext;

        public SnapShotMaterializer(ISnapShotStore snapShotStore, BoundedContext boundedContext)
        {
            this.snapShotStore = snapShotStore;
            this.boundedContext = boundedContext;
        }

        public void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent)
        {
            var snapShot = aggregate.GetSnapshot();
            this.snapShotStore.SaveSnapshot(aggregate.GetAggregateIdentity(this.boundedContext), snapShot);
        }

        public ChosenAggregateTypes ChosenAggregateTypes { get { return new ChosenAggregateTypes(); } }
    }
}
