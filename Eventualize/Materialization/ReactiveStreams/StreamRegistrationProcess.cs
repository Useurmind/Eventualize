using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Materialization.ReactiveStreams
{
    /// <summary>
    /// This class is responsible for coordinating all stream subscriptions and the start of the event streaming via <see cref="IEventSourceFactory.ConnectAll"/>.
    /// </summary>
    public class StreamRegistrationProcess
    {
        private struct SubscriberWithProgressTask
        {
            public ISubscribeToEventStreams Subscriber;

            public Task<EventStreamIndex?> ProgressTask;
        }

        private IEventSourceFactory eventSourceFactory;

        private IEnumerable<ISubscribeToEventStreams> streamSubscribers;

        public StreamRegistrationProcess(IEventSourceFactory eventSourceFactory, IEnumerable<ISubscribeToEventStreams> streamSubscribers)
        {
            this.eventSourceFactory = eventSourceFactory;
            this.streamSubscribers = streamSubscribers;
        }

        /// <summary>
        /// Perform all subscription logic and start the event streaming.
        /// </summary>
        /// <returns>Task because it is async.</returns>
        public async Task PerformSubscriptionsAndConnectAsync()
        {
            var subsribersWithProgressTasks = this.streamSubscribers.Select(x => new SubscriberWithProgressTask()
            {
                Subscriber = x,
                ProgressTask = x.GetLastHandledEventIndexAsync()
            });

            // getting the progress will most probably be a network action calculating something on a database or whatnot
            // therefore we do it async here
            await Task.WhenAll(subsribersWithProgressTasks.Select(x => x.ProgressTask));

            // the rest can be done as usual
            foreach (var progresses in subsribersWithProgressTasks)
            {
                var lastHandledEventIndex = progresses.ProgressTask.Result;
                var streamProvider = new EventSourceProvider(this.eventSourceFactory, lastHandledEventIndex);

                progresses.Subscriber.SubscribeStreams(streamProvider);
            }

            this.eventSourceFactory.ConnectAll();
        }
    }
}
