using System;
using System.Linq;

using Eventualize.Materialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;
using Eventualize.Persistence.Snapshots;

namespace Eventualize.Infrastructure
{
    public interface IEventualizeContainerBuilder
    {
        IEventualizeContainerBuilder RegisterNamedFactory<TInterface>(string name, Func<IEventualizeContainer, TInterface> createInstance);

        IEventualizeContainerBuilder RegisterSingleInstance<TInterface>(Func<IEventualizeContainer, TInterface> createInstance);

        IEventualizeContainerBuilder SetAggregateEventStoreFactory(
            Func<IEventualizeContainer, IAggregateEventStore> createAggregateEventStore);

        IEventualizeContainerBuilder SetLoggerFactory(Func<IEventualizeContainer, IEventualizeLogger> createLogger);

        IEventualizeContainerBuilder SetRepositoryFactory(Func<IEventualizeContainer, IAggregateRepository> createRepository);

        IEventualizeContainerBuilder SetSnapShotStoreFactory(Func<IEventualizeContainer, ISnapShotStore> createSnapShotStore);

        IEventualizeContainerBuilder SetMaterializationProgessStoreFactory(Func<IEventualizeContainer, IMaterializationProgessStore> createProgessStore);

        IEventualizeContainerBuilder SetMaterializerFactory(Func<IEventualizeContainer, IMaterializationEventPoller> createMaterializer);

        IEventualizeContainerBuilder SetAggregateFactoryFactory(Func<IEventualizeContainer, IConstructInstances> createAggregateFactory);

        IEventualizeContainerBuilder SetSnapshotConverterFactory(Func<IEventualizeContainer, ISnapshotConverter> createSnapshotConverter);

        IEventualizeContainerBuilder SetEventConverterFactory(Func<IEventualizeContainer, IEventConverter> createEventConverter);

        IEventualizeContainerBuilder SetSerializerFactory(Func<IEventualizeContainer, ISerializer> createSerializer);

        IEventualizeContainerBuilder AddMaterializationStrategyFactory(
            Func<IEventualizeContainer, IMaterializationStrategy> createMaterializationStrategy);

        IEventualizeContainerBuilder AddAggregateMaterializationStrategyFactory(
            Func<IEventualizeContainer, IAggregateMaterializationStrategy> createAggregateMaterializationStrategy);
    }
}