using System.Collections.Generic;
using System.Linq;

using Autofac;

using Eventualize.Infrastructure;
using Eventualize.Materialization;
using Eventualize.Materialization.AggregateMaterialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;
using Eventualize.Persistence.Snapshots;

namespace Eventualize.Autofac.Infrastructure
{
    public class AutofacComponentContext : IEventualizeContainer
    {
        private IComponentContext componentContext;

        public AutofacComponentContext(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        public IEventualizeLogger Logger
        {
            get
            {
                return this.componentContext.Resolve<IEventualizeLogger>();
            }
        }

        public IMaterializationProgessStore MaterializationProgessStore
        {
            get
            {
                return this.componentContext.Resolve<IMaterializationProgessStore>();
            }
        }

        public IMaterializationEventPoller MaterializationEventPoller
        {
            get
            {
                return this.componentContext.Resolve<IMaterializationEventPoller>();
            }
        }

        public IAggregateRepository AggregateRepository
        {
            get
            {
                return this.componentContext.Resolve<IAggregateRepository>();
            }
        }

        public IConstructInstances AggregateFactory
        {
            get
            {
                return this.componentContext.Resolve<IConstructInstances>();
            }
        }

        public IEventConverter EventConverter
        {
            get
            {
                return this.componentContext.Resolve<IEventConverter>();
            }
        }

        public ISerializer Serializer
        {
            get
            {
                return this.componentContext.Resolve<ISerializer>();
            }
        }

        public IEnumerable<IMaterializationStrategy> MaterializationStrategies
        {
            get
            {
                return this.componentContext.Resolve<IEnumerable<IMaterializationStrategy>>();
            }
        }

        public IEnumerable<IAggregateMaterializationStrategy> AggregateMaterializationStrategies
        {
            get
            {
                return this.componentContext.Resolve<IEnumerable<IAggregateMaterializationStrategy>>();
            }
        }

        public TInterface Resolve<TInterface>()
        {
            return this.componentContext.Resolve<TInterface>();
        }

        public IAggregateEventStore AggregateEventStore
        {
            get
            {
                return this.componentContext.Resolve<IAggregateEventStore>();
            }
        }

        public ISnapShotStore SnapShotStore
        {
            get
            {
                return this.componentContext.Resolve<ISnapShotStore>();
            }
        }

        public ISnapshotConverter SnapshotConverter
        {
            get
            {
                return this.componentContext.Resolve<ISnapshotConverter>();
            }
        }
    }
}