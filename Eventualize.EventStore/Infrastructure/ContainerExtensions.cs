using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.EventStore.Materialization;
using Eventualize.EventStore.Persistence;
using Eventualize.EventStore.Persistence.SnapShots;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Materialization;
using Eventualize.Materialization.Progress;
using Eventualize.Security;

namespace Eventualize.EventStore.Infrastructure
{
    public static class ContainerExtensions
    {
        public static IEventualizeContainerBuilder ConnectEventStore(
            this IEventualizeContainerBuilder containerBuilder,
            Func<IEventualizeContainer, IEventStoreConnection> createConnection)
        {
            containerBuilder.RegisterSingleInstance(x => new ConnectionEventHandler(x.Logger));

            return containerBuilder.RegisterSingleInstance(
                c =>
                {
                    var connection = createConnection(c);

                    var connectionEventHandler = c.Resolve<ConnectionEventHandler>();
                    connectionEventHandler.SetConnection(connection);

                    connection.ConnectAsync().Wait();
                    return connection;
                });
        }

        public static IEventualizeContainerBuilder ConnectEventStore(this IEventualizeContainerBuilder containerBuilder, Uri uri, ConnectionSettings connectionSettings)
        {
            return containerBuilder.ConnectEventStore(c => EventStoreConnection.Create(connectionSettings, uri));
        }

        public static IEventualizeContainerBuilder StoreSnapShotsInEventStore(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.SetSnapShotStoreFactory(c => new EventStoreSnapShotStore(c.Resolve<IEventStoreConnection>(), c.SnapshotConverter));
        }

        public static IEventualizeContainerBuilder StoreAggregatesInEventStore(this IEventualizeContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterSingleInstance(c => (IEventStoreEventConverter)new EventStoreEventConverter(c.EventConverter, c.Serializer));

            containerBuilder.SetAggregateEventStoreFactory(
                c =>
                    new AggregateEventStoreImplementation(c.Resolve<IEventStoreConnection>(), c.Resolve<IEventStoreEventConverter>()));

            return containerBuilder;
        }

        public static IEventualizeContainerBuilder MaterializeFromEventStore(this IEventualizeContainerBuilder containerBuilder)
        {
            containerBuilder.SetMaterializerFactory(
                   c =>
                       (IMaterializationEventPoller)
                       new EventStoreMaterializationEventPoller(
                           c.AggregateFactory,
                           c.Resolve<IEventStoreEventConverter>(),
                           c.Resolve<IEventStoreConnection>(),
                           c.MaterializationStrategies,
                           new KeyedMaterializationProgess(c.MaterializationProgessStore, "All"),
                           c.AggregateMaterializationStrategies,
                           EventualizeContext.Current.DefaultBoundedContext));

            return containerBuilder;
        }
    }
}
