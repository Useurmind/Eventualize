using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.Core;
using Eventualize.Infrastructure;
using Eventualize.Materialization;
using Eventualize.NEventStore.Materialization;
using Eventualize.NEventStore.Persistence;

using NEventStore;

namespace Eventualize.NEventStore.Infrastructure
{
    public static class ContainerExtensions
    {
        public static IEventualizeContainerBuilder ConnectNEventStore(this IEventualizeContainerBuilder containerBuilder, Func<IEventualizeContainer, IStoreEvents> createStore)
        {
            return containerBuilder.RegisterSingleInstance(createStore);
        }

        public static IEventualizeContainerBuilder StoreAggregatesInNEventStore(this IEventualizeContainerBuilder containerBuilder)
        {
            containerBuilder.SetRepositoryFactory(
                c =>
                    new NEventStoreRepository(c.Resolve<IStoreEvents>(), c.AggregateFactory, new ConflictDetector()));

            return containerBuilder;
        }

        public static IEventualizeContainerBuilder MaterializeFromNEventStore(this IEventualizeContainerBuilder containerBuilder)
        {
            containerBuilder.SetMaterializerFactory(
                   c =>
                       (IMaterializer)
                       new NEventStoreMaterializer(
                           c.AggregateFactory,
                           c.Resolve<IStoreEvents>(),
                           c.MaterializationStrategies));

            return containerBuilder;
        }
    }
}
