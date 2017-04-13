using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Materialization;
using Eventualize.Materialization.AggregateMaterialization;

namespace Eventualize.Persistence.Snapshots
{
    public class SnapShotMaterializer : IAggregateMaterializer
    {
        private ISnapShotStore snapShotStore;

        private EventNamespace eventNamespace;

        public SnapShotMaterializer(ISnapShotStore snapShotStore, EventNamespace eventNamespace)
        {
            this.snapShotStore = snapShotStore;
            this.eventNamespace = eventNamespace;
        }

        public void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent)
        {
            var snapShot = aggregate.GetSnapshot();
            this.snapShotStore.SaveSnapshot(aggregate.GetAggregateIdentity(this.eventNamespace), snapShot);
        }

        public ChosenAggregateTypes ChosenAggregateTypes { get { return new ChosenAggregateTypes(); } }
    }
}
