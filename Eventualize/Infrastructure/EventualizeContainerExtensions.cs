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
using Eventualize.Interfaces.Materialization.Fluent;
using Eventualize.Materialization;
using Eventualize.Materialization.AggregateMaterialization;
using Eventualize.Materialization.Fluent;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;
using Eventualize.Security;
using Eventualize.Snapshots;

namespace Eventualize.Infrastructure
{
    /// <summary>
    /// Extensions to the container that perform whole registration operations.
    /// </summary>
    public static class EventualizeContainerExtensions
    {
        /// <summary>
        /// Store the aggregates in an in memory event store (see <see cref="InMemoryAggregateEventStore"/>).
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public static IEventualizeContainerBuilder StoreAggregatesInMemory(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.SetAggregateEventStoreFactory(c => new InMemoryAggregateEventStore(c.DomainIdentityProvider));
        }

        /// <summary>
        /// Construct the domain model using a mix of reflection and attributes.
        /// The domain model will follow the following rules:
        /// - All classes that implement <see cref="IAggregate"/> or <see cref="IEventData"/> will be aggregates and events
        /// - If an attribute of type <see cref="AggregateTypeNameAttribute"/>, <see cref="EventTypeNameAttribute"/>, <see cref="BoundedContextAttribute"/> is applied it will force the given string to the corresponding name
        /// - Without attributes aggregates and events will be named according to their class name
        /// - Without attribute aggregates/events will belong to a bounded context that is equivalent to the part of the namespace two levels above the aggregate/event
        /// The last rule can be understood when thinking about aggregates as a collection of an aggregate, its state, and several events that it can raise/apply.
        /// Therefore, we assume you put all those classes into one folder.
        /// The folder above that folder defines the bounded context for the aggregates and events.
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="assemblies">The assemblies that should be scanned for aggregates and events.</param>
        /// <returns></returns>
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
                                   .SetRepositoryFactory(c => new AggregateRepository(c.AggregateEventStore, c.DomainIdentityProvider, c.AggregateFactory, c.SnapShotStore, c.Resolve<RepositoryOptions>()))
                                   .RegisterSingleInstance<IMaterializationStrategy>(c => new FluentProjectionMaterializionStrategy(
                                       c.Resolve<IEnumerable<IEventMaterializationAction>>(),
                                       c.Resolve<IEnumerable<IEventMaterializationActionHandler>>()));
        }

        public static IMaterializationFactory Materialize(this IEventualizeContainerBuilder containerBuilder)
        {
            return new MaterializationFactory(containerBuilder);
        }

        public static IEventualizeContainerBuilder MaterializeInMemory(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.AddAggregateMaterializationStrategyFactory(c => new InMemoryMaterializationStrategy(c.AggregateFactory));
        }

        public static IEventualizeContainerBuilder StoreMaterializationProgessInFileSystem(
            this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.SetMaterializationProgessStoreFactory(c => new FileMaterializationProgessStore(c.Serializer));
        }

        public static IEventualizeContainerBuilder MaterializePerAggregate(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.AddAggregateMaterializationStrategyFactory(c => new AggregateMaterializationStrategy(c.AggregateRepository, c.DomainIdentityProvider, c.Resolve<IEnumerable<IAggregateMaterializer>>()));
        }
    }
}