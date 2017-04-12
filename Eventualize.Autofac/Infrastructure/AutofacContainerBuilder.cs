using System;
using System.Linq;

using Autofac;

using Eventualize.Infrastructure;
using Eventualize.Materialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;

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
            this.builder.Register(ctx => createLogger(ctx.Eventualize())).As<IEventualizeLogger>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder SetRepositoryFactory(Func<IEventualizeContainer, IAggregateRepository> createRepository)
        {
            this.builder.Register(ctx => createRepository(ctx.Eventualize())).As<IAggregateRepository>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder SetMaterializerFactory(Func<IEventualizeContainer, IMaterializationEventPoller> createMaterializer)
        {
            this.builder.Register(ctx => createMaterializer(ctx.Eventualize())).As<IMaterializationEventPoller>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder SetAggregateFactoryFactory(Func<IEventualizeContainer, IConstructInstances> createAggregateFactory)
        {
            this.builder.Register(ctx => createAggregateFactory(ctx.Eventualize())).As<IConstructInstances>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder SetEventConverterFactory(Func<IEventualizeContainer, IEventConverter> createEventConverter)
        {
            this.builder.Register(ctx => createEventConverter(ctx.Eventualize())).As<IEventConverter>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder SetSerializerFactory(Func<IEventualizeContainer, ISerializer> createSerializer)
        {
            this.builder.Register(ctx => createSerializer(ctx.Eventualize())).As<ISerializer>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder AddMaterializationStrategyFactory(Func<IEventualizeContainer, IMaterializationStrategy> createMaterializationStrategy)
        {
            this.builder.Register(ctx => createMaterializationStrategy(ctx.Eventualize())).As<IMaterializationStrategy>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder AddAggregateMaterializationStrategyFactory(Func<IEventualizeContainer, IAggregateMaterializationStrategy> createAggregateMaterializationStrategy)
        {
            this.builder.Register(ctx => createAggregateMaterializationStrategy(ctx.Eventualize())).As<IAggregateMaterializationStrategy>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder RegisterSingleInstance<TInterface>(Func<IEventualizeContainer, TInterface> createInstance)
        {
            this.builder.Register(x => createInstance(x.Eventualize())).As<TInterface>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder SetAggregateEventStoreFactory(Func<IEventualizeContainer, IAggregateEventStore> createAggregateEventStore)
        {
            this.builder.Register(x => createAggregateEventStore(x.Eventualize())).As<IAggregateEventStore>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder SetMaterializationProgessStoreFactory(Func<IEventualizeContainer, IMaterializationProgessStore> createProgessStore)
        {
            this.builder.Register(x => createProgessStore(x.Eventualize())).As<IMaterializationProgessStore>().SingleInstance();
            return this;
        }
    }
}