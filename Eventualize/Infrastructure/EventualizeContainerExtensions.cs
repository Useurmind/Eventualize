using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Materialization;
using Eventualize.Materialization.AggregateMaterialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;
using Eventualize.Persistence.Snapshots;
using Eventualize.Security;

namespace Eventualize.Infrastructure
{
    public static class EventualizeContainerExtensions
    {
        public static IEventualizeContainerBuilder MaterializeSnapShots<TAggregate>(this IEventualizeContainerBuilder containerBuilder, EventNamespace? eventNamespace = null)
            where TAggregate : class, IAggregate
        {
            return containerBuilder.RegisterSingleInstance<IAggregateMaterializer>(c => new SnapShotMaterializer(c.SnapShotStore, EventualizeContext.TakeThisOrDefault(eventNamespace)));
        }

        public static IEventualizeContainerBuilder SetDefaults(this IEventualizeContainerBuilder containerBuilder, params Assembly[] domainAssemblies)
        {
            return containerBuilder.SetAggregateFactoryFactory(
                                       c =>
                                       {
                                           var factory = new InstanceFactory(c.Serializer);
                                           factory.ScanAggregateTypes(domainAssemblies);
                                           return factory;
                                       })
                                    .SetSnapshotConverterFactory(
                                       c =>
                                       {
                                           var snapshotConverter = new SnapshotConverter(c.Serializer);
                                           snapshotConverter.ScanSnapshotTypes(domainAssemblies);
                                           return snapshotConverter;
                                       })
                                   .SetEventConverterFactory(
                                       c =>
                                       {
                                           var eventConverter = new EventConverter(c.Serializer);
                                           eventConverter.ScanEventTypes(domainAssemblies);
                                           return eventConverter;
                                       })
                                   .SetSerializerFactory(c => new JsonSerializer())
                                   .SetLoggerFactory(c => new ConsoleLogger())
                                   .SetRepositoryFactory(c => new AggregateRepository(c.AggregateEventStore, c.AggregateFactory, c.SnapShotStore));
        }

        public static IEventualizeContainerBuilder MaterializeInMemory(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.AddAggregateMaterializationStrategyFactory(c => new InMemoryMaterialization(c.AggregateFactory));
        }

        public static IEventualizeContainerBuilder StoreMaterializationProgessInFileSystem(
            this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.SetMaterializationProgessStoreFactory(c => new FileMaterializationProgessStore(c.Serializer));
        }

        public static IEventualizeContainerBuilder MaterializePerAggregate(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.AddAggregateMaterializationStrategyFactory(c => new AggregateMaterializationDistributor(c.AggregateRepository, c.Resolve<IEnumerable<IAggregateMaterializer>>()));
        }
    }
}