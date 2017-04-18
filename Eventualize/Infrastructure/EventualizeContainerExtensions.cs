using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Domain.MetaModel;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
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
        public static IEventualizeContainerBuilder ConstructDomainModelViaReflection(this IEventualizeContainerBuilder containerBuilder, IEnumerable<Assembly> assemblies = null)
        {
            if (assemblies == null)
            {
                assemblies = new Assembly[] {Assembly.GetCallingAssembly()};
            }

            return containerBuilder.SetDomainMetaModelFactory(
                x =>
                {
                    var metaModelFactory = new ReflectionBasedMetaModelFactory(assemblies);

                    return metaModelFactory.Build();
                });
        }

        public static IEventualizeContainerBuilder MaterializeSnapShots<TAggregate>(this IEventualizeContainerBuilder containerBuilder, BoundedContextName? boundedContextName = null)
            where TAggregate : class, IAggregate
        {
            return containerBuilder.RegisterSingleInstance<IAggregateMaterializer>(c => new SnapShotMaterializer(c.SnapShotStore, c.DomainIdentityProvider));
        }

        public static IEventualizeContainerBuilder DeriveIdentitiesFromAttributes(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.SetDomainIdentityProviderFactory(x => new DomainModelIdentityProvider(x.DomainMetaModel));
        }

        public static IEventualizeContainerBuilder SetDefaults(this IEventualizeContainerBuilder containerBuilder, params Assembly[] domainAssemblies)
        {
            containerBuilder.RegisterSingleInstance(c => new RepositoryOptions() { PageSize = 1024 });

            return containerBuilder.SetAggregateFactoryFactory(
                                       c =>
                                       {
                                           var factory = new AggregateFactory(c.DomainMetaModel);
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