using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.EventStore.Persistence;
using Eventualize.EventStore.Projections;
using Eventualize.EventStore.Test.Projections;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Materialization.ReactiveStreams;

namespace Eventualize.EventStore.Materialization
{
    public class EventStoreEventSourceFactory : IEventSourceFactory
    {
        private IDictionary<string, EventStoreStreamPoller> eventPollerByStreamName;
        private IDictionary<string, IConnectableObservable<IEvent>> observablesByStreamName;
        private IDictionary<string, IDisposable> connectionsByStreamName;

        private IEventStoreEventConverter eventConverter;

        private IEventStoreConnection connection;

        private IProjectionFactory projectionFactory;

        public EventStoreEventSourceFactory(IEventStoreEventConverter eventConverter, IEventStoreConnection connection, IProjectionFactory projectionFactory)
        {
            this.eventConverter = eventConverter;
            this.connection = connection;
            this.projectionFactory = projectionFactory;
            this.eventPollerByStreamName = new Dictionary<string, EventStoreStreamPoller>();
            this.observablesByStreamName = new Dictionary<string, IConnectableObservable<IEvent>>();
            this.connectionsByStreamName = new Dictionary<string, IDisposable>();
        }

        public void ConnectAll()
        {
            foreach (var kv in this.observablesByStreamName)
            {
                var streamName = kv.Key;
                var observable = kv.Value;

                this.connectionsByStreamName.Add(streamName, observable.Connect());
            }
        }

        public IEventSource FromAll(EventStreamIndex? afterEventIndex = null)
        {
            this.projectionFactory.EnsureProjectionForAllBoundedContexts();

            var streamName = new ProjectionStreamName().ToString();

            return this.GetEventSourceForStream(streamName, afterEventIndex);
        }

        public IEventSource FromBoundedContext(BoundedContextName boundedContextName, EventStreamIndex? afterEventIndex = null)
        {
            this.projectionFactory.EnsureProjectionFor(boundedContextName);

            var streamName = new ProjectionStreamName(boundedContextName).ToString();

            return this.GetEventSourceForStream(streamName, afterEventIndex);
        }

        public IAggregateEventSource FromAggregateType(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName, EventStreamIndex? afterEventIndex = null)
        {
            this.projectionFactory.EnsureProjectionFor(boundedContextName, aggregateTypeName);

            var streamName = new ProjectionStreamName(boundedContextName, aggregateTypeName).ToString();

            return this.GetEventSourceForStream(streamName, afterEventIndex).AsAggregateEventSource();
        }

        public IEventSource FromEventType(BoundedContextName boundedContextName, EventTypeName eventTypeName, EventStreamIndex? afterEventIndex = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This methods creates only one poller for each stream.
        /// The poller 
        /// </summary>
        /// <param name="streamName"></param>
        /// <param name="afterEventIndex"></param>
        /// <returns></returns>
        private IEventSource GetEventSourceForStream(string streamName, EventStreamIndex? afterEventIndex = null)
        {
            if (this.connectionsByStreamName.ContainsKey(streamName))
            {
                throw new Exception($"Event store stream {streamName} is already connected. You cannot subscribe to it without losing events. Please register your subscribes before ConnectAll is called on the IEventSourceFactory.");
            }

            EventStoreStreamPoller eventSource = null;
            IConnectableObservable<IEvent> observable = null;
            if (!this.eventPollerByStreamName.TryGetValue(streamName, out eventSource))
            {
                eventSource = new EventStoreStreamPoller(this.eventConverter, this.connection, streamName);

                observable = eventSource.Publish();

                this.eventPollerByStreamName.Add(streamName, eventSource);
                this.observablesByStreamName.Add(streamName, observable);
            }
            else
            {
                observable = this.observablesByStreamName[streamName];
            }

            eventSource.AddAdditionalEventStreamIndex(afterEventIndex);

            // make sure only events starting from the requested index are delivered
            // this is necessary because different subscribers could want to start at 
            return new WrapperEventSource(observable).SkipAfter(afterEventIndex);
        }
    }
}
