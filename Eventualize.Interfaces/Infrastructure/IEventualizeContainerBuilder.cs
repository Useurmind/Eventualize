using System;
using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Persistence;
using Eventualize.Interfaces.Snapshots;

namespace Eventualize.Interfaces.Infrastructure
{
    public interface IEventualizeContainerBuilder
    {
        IEventualizeContainerBuilder RegisterNamedFactory<TInterface>(string name, Func<IEventualizeContainer, TInterface> createInstance);

        IEventualizeContainerBuilder RegisterSingleInstance<TInterface>(Func<IEventualizeContainer, TInterface> createInstance);

        IEventualizeContainerBuilder SetDomainMetaModelFactory(
           Func<IEventualizeContainer, IDomainMetaModel> createDomainMetaModel);

        IEventualizeContainerBuilder SetDomainIdentityProviderFactory(
            Func<IEventualizeContainer, IDomainIdentityProvider> createDomainIdentityProvider);

        IEventualizeContainerBuilder SetAggregateEventStoreFactory(
            Func<IEventualizeContainer, IAggregateEventStore> createAggregateEventStore);

        IEventualizeContainerBuilder SetLoggerFactory(Func<IEventualizeContainer, IEventualizeLogger> createLogger);

        IEventualizeContainerBuilder SetRepositoryFactory(Func<IEventualizeContainer, IAggregateRepository> createRepository);

        IEventualizeContainerBuilder SetSnapShotStoreFactory(Func<IEventualizeContainer, ISnapShotStore> createSnapShotStore);

        IEventualizeContainerBuilder SetMaterializationProgessStoreFactory(Func<IEventualizeContainer, IMaterializationProgessStore> createProgessStore);

        IEventualizeContainerBuilder SetMaterializerFactory(Func<IEventualizeContainer, IMaterializationEventPoller> createMaterializer);

        IEventualizeContainerBuilder SetAggregateFactoryFactory(Func<IEventualizeContainer, IAggregateFactory> createAggregateFactory);

        IEventualizeContainerBuilder SetSnapshotConverterFactory(Func<IEventualizeContainer, ISnapshotConverter> createSnapshotConverter);

        IEventualizeContainerBuilder SetEventConverterFactory(Func<IEventualizeContainer, IEventConverter> createEventConverter);

        IEventualizeContainerBuilder SetSerializerFactory(Func<IEventualizeContainer, ISerializer> createSerializer);

        IEventualizeContainerBuilder AddMaterializationStrategyFactory(
            Func<IEventualizeContainer, IMaterializationStrategy> createMaterializationStrategy);

        IEventualizeContainerBuilder AddAggregateMaterializationStrategyFactory(
            Func<IEventualizeContainer, IAggregateMaterializationStrategy> createAggregateMaterializationStrategy);
    }
}