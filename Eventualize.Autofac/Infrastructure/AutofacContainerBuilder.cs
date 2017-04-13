using System;
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
    public class AutofacContainerBuilder : IEventualizeContainerBuilder
    {
        private ContainerBuilder builder;

        public AutofacContainerBuilder(ContainerBuilder builder)
        {
            this.builder = builder;
        }

        public IEventualizeContainerBuilder SetLoggerFactory(Func<IEventualizeContainer, IEventualizeLogger> createLogger)
        {
            return this.RegisterFuncFactorySingleton(createLogger);
        }

        public IEventualizeContainerBuilder SetRepositoryFactory(Func<IEventualizeContainer, IAggregateRepository> createRepository)
        {
            return this.RegisterFuncFactorySingleton(createRepository);
        }

        public IEventualizeContainerBuilder SetMaterializerFactory(Func<IEventualizeContainer, IMaterializationEventPoller> createMaterializer)
        {
            return this.RegisterFuncFactorySingleton(createMaterializer);
        }

        public IEventualizeContainerBuilder SetAggregateFactoryFactory(Func<IEventualizeContainer, IConstructInstances> createAggregateFactory)
        {
            return this.RegisterFuncFactorySingleton(createAggregateFactory);
        }

        public IEventualizeContainerBuilder SetEventConverterFactory(Func<IEventualizeContainer, IEventConverter> createEventConverter)
        {
            return this.RegisterFuncFactorySingleton(createEventConverter);
        }

        public IEventualizeContainerBuilder SetSerializerFactory(Func<IEventualizeContainer, ISerializer> createSerializer)
        {
            return this.RegisterFuncFactorySingleton(createSerializer);
        }

        public IEventualizeContainerBuilder AddMaterializationStrategyFactory(Func<IEventualizeContainer, IMaterializationStrategy> createMaterializationStrategy)
        {
            return this.RegisterFuncFactorySingleton(createMaterializationStrategy);
        }

        public IEventualizeContainerBuilder AddAggregateMaterializationStrategyFactory(Func<IEventualizeContainer, IAggregateMaterializationStrategy> createAggregateMaterializationStrategy)
        {
            return this.RegisterFuncFactorySingleton(createAggregateMaterializationStrategy);
        }

        public IEventualizeContainerBuilder RegisterSingleInstance<TInterface>(Func<IEventualizeContainer, TInterface> createInstance)
        {
            return this.RegisterFuncFactorySingleton(createInstance);
        }

        public IEventualizeContainerBuilder SetAggregateEventStoreFactory(Func<IEventualizeContainer, IAggregateEventStore> createAggregateEventStore)
        {
            return this.RegisterFuncFactorySingleton(createAggregateEventStore);
        }

        public IEventualizeContainerBuilder SetMaterializationProgessStoreFactory(Func<IEventualizeContainer, IMaterializationProgessStore> createProgessStore)
        {
            return this.RegisterFuncFactorySingleton(createProgessStore);
        }

        public IEventualizeContainerBuilder SetSnapShotStoreFactory(Func<IEventualizeContainer, ISnapShotStore> createSnapShotStore)
        {
            return this.RegisterFuncFactorySingleton(createSnapShotStore);
        }

        public IEventualizeContainerBuilder SetSnapshotConverterFactory(Func<IEventualizeContainer, ISnapshotConverter> createSnapshotConverter)
        {
            return this.RegisterFuncFactorySingleton(createSnapshotConverter);
        }

        private IEventualizeContainerBuilder RegisterFuncFactorySingleton<T>(Func<IEventualizeContainer, T> create)
        {
            this.builder.Register(x => create(x.Eventualize())).As<T>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder RegisterNamedFactory<TInterface>(string name, Func<IEventualizeContainer, TInterface> createInstance)
        {
            this.builder.Register(x => createInstance(x.Eventualize())).Named<TInterface>(name);
            return this;
        }
    }
}