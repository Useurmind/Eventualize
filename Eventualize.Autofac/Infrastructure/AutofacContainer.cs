using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Eventualize.Infrastructure;
using Eventualize.Materialization;
using Eventualize.Persistence;

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

        public IAggregateRepository AggregateRepository
        {
            get
            {
                return this.container.Resolve<IAggregateRepository>();
            }
        }

        public IConstructInstances AggregateFactory
        {
            get
            {
                return this.container.Resolve<IConstructInstances>();
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
    }
}
