using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Materialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;

namespace Eventualize.Infrastructure
{
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
                                   .SetLoggerFactory(c => new ConsoleLogger())
                                   .SetRepositoryFactory(c => new AggregateRepository(c.AggregateEventStore, c.AggregateFactory));
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
            return containerBuilder.AddAggregateMaterializationStrategyFactory(c => new AggregateMaterializationDistributor(c.AggregateRepository, c.Resolve<IEnumerable<IAggregateMaterializer>>()));
        }
    }
}