using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Materialization;

namespace Eventualize.Persistence.Snapshots
{
    public class SnapShotMaterializer<TAggregate> : SingleAggregateMaterializerBase<TAggregate>
        where TAggregate : class, IAggregate
    {
        private ISnapShotStore snapShotStore;

        private EventNamespace eventNamespace;

        public SnapShotMaterializer(ISnapShotStore snapShotStore, EventNamespace eventNamespace)
        {
            this.snapShotStore = snapShotStore;
            this.eventNamespace = eventNamespace;
        }

        protected override void HandleAggregateEvent(TAggregate aggregate, IAggregateEvent materializationEvent)
        {
            var snapShot = aggregate.GetSnapshot();
            this.snapShotStore.SaveSnapshot(aggregate.GetAggregateIdentity(this.eventNamespace), snapShot);
        }
    }
}
