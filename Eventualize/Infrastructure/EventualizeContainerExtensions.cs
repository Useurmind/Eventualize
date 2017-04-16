using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Materialization;
using Eventualize.Materialization;
using Eventualize.Materialization.AggregateMaterialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;
using Eventualize.Security;
using Eventualize.Snapshots;

namespace Eventualize.Infrastructure
{
    public static class EventualizeContainerExtensions
    {
        public static IEventualizeContainerBuilder MaterializeSnapShots<TAggregate>(this IEventualizeContainerBuilder containerBuilder, BoundedContext? boundedContext = null)
            where TAggregate : class, IAggregate
        {
            return containerBuilder.RegisterSingleInstance<IAggregateMaterializer>(c => new SnapShotMaterializer(c.SnapShotStore, c.DomainIdentityProvider));
        }

        public static IEventualizeContainerBuilder DeriveIdentitiesFromAttributes(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.SetDomainIdentityProviderFactory(x => new AttributeBasedIdentityProvider());
        }

        public static IEventualizeContainerBuilder SetDefaults(this IEventualizeContainerBuilder containerBuilder, params Assembly[] domainAssemblies)
        {
            containerBuilder.RegisterSingleInstance(c => new RepositoryOptions() { PageSize = 1024 });

            return containerBuilder.SetAggregateFactoryFactory(
                                       c =>
                                       {
                                           var factory = new AggregateFactory(c.Serializer);
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
                                   .SetRepositoryFactory(c => new AggregateRepository(c.AggregateEventStore, c.DomainIdentityProvider, c.AggregateFactory, c.SnapShotStore, c.Resolve<RepositoryOptions>()));
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
            return containerBuilder.AddAggregateMaterializationStrategyFactory(c => new AggregateMaterializationDistributor(c.AggregateRepository, c.DomainIdentityProvider, c.Resolve<IEnumerable<IAggregateMaterializer>>()));
        }
    }
}