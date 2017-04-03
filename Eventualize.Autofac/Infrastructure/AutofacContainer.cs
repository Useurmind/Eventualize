using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using Eventualize.Infrastructure;
using Eventualize.Materialization;
using Eventualize.Persistence;

namespace Eventualize.Autofac.Infrastructure
{


    public class AutofacContainer : IEventualizeContainer
    {
        private IContainer container;

        public AutofacContainer(IContainer container)
        {
            this.container = container;
        }

        public IEventualizeLogger Logger
        {
            get
            {
                return this.container.Resolve<IEventualizeLogger>();
            }
        }

        public IMaterializer Materializer
        {
            get
            {
                return this.container.Resolve<IMaterializer>();
            }
        }

        public IRepository Repository
        {
            get
            {
                return this.container.Resolve<IRepository>();
            }
        }

        public IConstructInstances AggregateFactory
        {
            get
            {
                return this.container.Resolve<IConstructInstances>();
            }
        }

        public IEventConverter EventConverter
        {
            get
            {
                return this.container.Resolve<IEventConverter>();
            }
        }

        public ISerializer Serializer
        {
            get
            {
                return this.container.Resolve<ISerializer>();
            }
        }

        public IEnumerable<IMaterializationStrategy> MaterializationStrategies
        {
            get
            {
                return this.container.Resolve<IEnumerable<IMaterializationStrategy>>();
            }
        }

        public TInterface Resolve<TInterface>()
        {
            return this.container.Resolve<TInterface>();
        }
    }

    public class AutofacComponentContext : IEventualizeContainer
    {
        private IComponentContext componentContext;
        
        public AutofacComponentContext(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        public IEventualizeLogger Logger
        {
            get
            {
                return this.componentContext.Resolve<IEventualizeLogger>();
            }
        }

        public IMaterializer Materializer
        {
            get
            {
                return this.componentContext.Resolve<IMaterializer>();
            }
        }

        public IRepository Repository
        {
            get
            {
                return this.componentContext.Resolve<IRepository>();
            }
        }

        public IConstructInstances AggregateFactory
        {
            get
            {
                return this.componentContext.Resolve<IConstructInstances>();
            }
        }

        public IEventConverter EventConverter
        {
            get
            {
                return this.componentContext.Resolve<IEventConverter>();
            }
        }

        public ISerializer Serializer
        {
            get
            {
                return this.componentContext.Resolve<ISerializer>();
            }
        }

        public IEnumerable<IMaterializationStrategy> MaterializationStrategies
        {
            get
            {
                return this.componentContext.Resolve<IEnumerable<IMaterializationStrategy>>();
            }
        }

        public TInterface Resolve<TInterface>()
        {
            return this.componentContext.Resolve<TInterface>();
        }
    }

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

        public IEventualizeContainerBuilder SetRepositoryFactory(Func<IEventualizeContainer, IRepository> createRepository)
        {
            this.builder.Register(ctx => createRepository(ctx.Eventualize())).As<IRepository>().SingleInstance();
            return this;
        }

        public IEventualizeContainerBuilder SetMaterializerFactory(Func<IEventualizeContainer, IMaterializer> createMaterializer)
        {
            this.builder.Register(ctx => createMaterializer(ctx.Eventualize())).As<IMaterializer>().SingleInstance();
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

        public IEventualizeContainerBuilder RegisterSingleInstance<TInterface>(Func<IEventualizeContainer, TInterface> createInstance)
        {
            this.builder.Register(x => createInstance(x.Eventualize())).As<TInterface>().SingleInstance();
            return this;
        }
    }

    public static class AutofacExtensions
    {
        public static IEventualizeContainer Eventualize(this IContainer container)
        {
            return new AutofacContainer(container);
        }

        public static IEventualizeContainer Eventualize(this IComponentContext componentContext)
        {
            return new AutofacComponentContext(componentContext);
        }

        public static IEventualizeContainerBuilder Eventualize(this ContainerBuilder containerBuilder)
        {
            return new AutofacContainerBuilder(containerBuilder);
        }

        public static ContainerBuilder Eventualize(this ContainerBuilder containerBuilder, Action<IEventualizeContainerBuilder> registerEventualize)
        {
            registerEventualize(new AutofacContainerBuilder(containerBuilder));
            return containerBuilder;
        }
    }
}
