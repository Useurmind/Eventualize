using System.Linq;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Snapshots;
using Eventualize.Materialization.AggregateMaterialization;

namespace Eventualize.Snapshots
{
    public class SnapShotMaterializer : IAggregateMaterializer
    {
        private ISnapShotStore snapShotStore;

        private IDomainIdentityProvider domainIdentityProvider;

        public SnapShotMaterializer(ISnapShotStore snapShotStore, IDomainIdentityProvider domainIdentityProvider)
        {
            this.snapShotStore = snapShotStore;
            this.domainIdentityProvider = domainIdentityProvider;
        }

        public void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent)
        {
            var snapShot = aggregate.GetSnapshot();
            this.snapShotStore.SaveSnapshot(this.domainIdentityProvider.GetAggregateIdentity(aggregate), snapShot);
        }

        public ChosenAggregateTypes ChosenAggregateTypes { get { return new ChosenAggregateTypes(); } }
    }


}
