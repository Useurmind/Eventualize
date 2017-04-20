using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Eventualize.Interfaces;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Progress;
using Eventualize.Interfaces.Persistence;
using Eventualize.Interfaces.Snapshots;

namespace Eventualize.Autofac.Infrastructure
{


    public class AutofacContainer : IEventualizeContainer
    {
        private IContainer container;

        public AutofacContainer(IContainer container)
        {
            this.container = container;
        }

        public IEventualizeLogger Logger
        {
            get
            {
                return this.container.Resolve<IEventualizeLogger>();
            }
        }

        public IMaterializationEventPoller MaterializationEventPoller
        {
            get
            {
                return this.container.Resolve<IMaterializationEventPoller>();
            }
        }

        public IMaterializationProgessStore MaterializationProgessStore
        {
            get
            {
                return this.container.Resolve<IMaterializationProgessStore>();
            }
        }

        public IAggregateRepository AggregateRepository
        {
            get
            {
                return this.container.Resolve<IAggregateRepository>();
            }
        }

        public IAggregateFactory AggregateFactory
        {
            get
            {
                return this.container.Resolve<IAggregateFactory>();
            }
        }

        public IEventConverter EventConverter
        {
            get
            {
                return this.container.Resolve<IEventConverter>();
            }
        }

        public ISerializer Serializer
        {
            get
            {
                return this.container.Resolve<ISerializer>();
            }
        }

        public IEnumerable<IMaterializationStrategy> MaterializationStrategies
        {
            get
            {
                return this.container.Resolve<IEnumerable<IMaterializationStrategy>>();
            }
        }

        public IEnumerable<IAggregateMaterializationStrategy> AggregateMaterializationStrategies
        {
            get
            {
                return this.container.Resolve<IEnumerable<IAggregateMaterializationStrategy>>();
            }
        }

        public TInterface Resolve<TInterface>()
        {
            return this.container.Resolve<TInterface>();
        }

        public IAggregateEventStore AggregateEventStore
        {
            get
            {
                return this.container.Resolve<IAggregateEventStore>();
            }
        }

        public ISnapShotStore SnapShotStore
        {
            get
            {
                return this.container.Resolve<ISnapShotStore>();
            }
        }

        public ISnapshotConverter SnapshotConverter
        {
            get
            {
                return this.container.Resolve<ISnapshotConverter>();
            }
        }

        public IDomainIdentityProvider DomainIdentityProvider
        {
            get
            {
                return this.container.Resolve<IDomainIdentityProvider>();
            }
        }

        public IDomainMetaModel DomainMetaModel
        {
            get
            {
                return this.container.Resolve<IDomainMetaModel>();
            }
        }
    }
}
