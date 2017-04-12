using System;
using System.Linq;

using Eventualize.Materialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;

namespace Eventualize.Infrastructure
{
    public interface IEventualizeContainerBuilder
    {
        IEventualizeContainerBuilder RegisterSingleInstance<TInterface>(Func<IEventualizeContainer, TInterface> createInstance);

        IEventualizeContainerBuilder SetAggregateEventStoreFactory(
            Func<IEventualizeContainer, IAggregateEventStore> createAggregateEventStore);

        IEventualizeContainerBuilder SetLoggerFactory(Func<IEventualizeContainer, IEventualizeLogger> createLogger);

        IEventualizeContainerBuilder SetRepositoryFactory(Func<IEventualizeContainer, IAggregateRepository> createRepository);

        IEventualizeContainerBuilder SetMaterializationProgessStoreFactory(Func<IEventualizeContainer, IMaterializationProgessStore> createProgessStore);

        IEventualizeContainerBuilder SetMaterializerFactory(Func<IEventualizeContainer, IMaterializationEventPoller> createMaterializer);

        IEventualizeContainerBuilder SetAggregateFactoryFactory(Func<IEventualizeContainer, IConstructInstances> createAggregateFactory);

        IEventualizeContainerBuilder SetEventConverterFactory(Func<IEventualizeContainer, IEventConverter> createEventConverter);

        IEventualizeContainerBuilder SetSerializerFactory(Func<IEventualizeContainer, ISerializer> createSerializer);

        IEventualizeContainerBuilder AddMaterializationStrategyFactory(
            Func<IEventualizeContainer, IMaterializationStrategy> createMaterializationStrategy);

        IEventualizeContainerBuilder AddAggregateMaterializationStrategyFactory(
            Func<IEventualizeContainer, IAggregateMaterializationStrategy> createAggregateMaterializationStrategy);
    }
}