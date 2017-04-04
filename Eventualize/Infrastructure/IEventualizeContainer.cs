using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Materialization;
using Eventualize.Persistence;

namespace Eventualize.Infrastructure
{
    public interface IEventualizeLogger
    {
        void Trace(string message);
    }

    public class ConsoleLogger : IEventualizeLogger
    {
        public void Trace(string message)
        {
            Console.WriteLine(message);
        }
    }

    public interface IEventualizeContainer
    {
        IEventualizeLogger Logger { get; }

        IMaterializer Materializer { get; }

        IRepository Repository { get; }

        IConstructInstances AggregateFactory { get; }

        IEventConverter EventConverter { get; }

        ISerializer Serializer { get; }

        IEnumerable<IMaterializationStrategy> MaterializationStrategies { get; }

        TInterface Resolve<TInterface>();
    }

    public interface IEventualizeContainerBuilder
    {
        IEventualizeContainerBuilder RegisterSingleInstance<TInterface>(Func<IEventualizeContainer, TInterface> createInstance);

        IEventualizeContainerBuilder SetLoggerFactory(Func<IEventualizeContainer, IEventualizeLogger> createLogger);

        IEventualizeContainerBuilder SetRepositoryFactory(Func<IEventualizeContainer, IRepository> createRepository);

        IEventualizeContainerBuilder SetMaterializerFactory(Func<IEventualizeContainer, IMaterializer> createMaterializer);

        IEventualizeContainerBuilder SetAggregateFactoryFactory(Func<IEventualizeContainer, IConstructInstances> createAggregateFactory);

        IEventualizeContainerBuilder SetEventConverterFactory(Func<IEventualizeContainer, IEventConverter> createEventConverter);

        IEventualizeContainerBuilder SetSerializerFactory(Func<IEventualizeContainer, ISerializer> createSerializer);

        IEventualizeContainerBuilder AddMaterializationStrategyFactory(
            Func<IEventualizeContainer, IMaterializationStrategy> createMaterializationStrategy);
    }

    public static class EventualizeContainerExtensions
    {
        public static IEventualizeContainerBuilder SetDefaults(this IEventualizeContainerBuilder containerBuilder, params Assembly[] domainAssemblies)
        {
            return containerBuilder.SetAggregateFactoryFactory(
                                       c =>
                                       {
                                           var factory = new InstanceFactory(c.Serializer);
                                           factory.ScanAggregateTypes(domainAssemblies);
                                           return factory;
                                       })
                .SetEventConverterFactory(
                                       c =>
                                       {
                                           var eventConverter = new EventConverter(c.Serializer);
                                           eventConverter.ScanEventTypes(domainAssemblies);
                                           return eventConverter;
                                       })
                .SetSerializerFactory(c => new JsonSerializer())
                .SetLoggerFactory(c => new ConsoleLogger());
        }

        public static IEventualizeContainerBuilder MaterializeInMemory(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.AddMaterializationStrategyFactory(c => new InMemoryMaterialization(c.AggregateFactory));
        }
    }
}
