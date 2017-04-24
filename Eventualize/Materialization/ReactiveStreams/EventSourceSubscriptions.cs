using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.Materialization;

namespace Eventualize.Materialization.ReactiveStreams
{
    public static class EventSourceSubscriptions
    {
        public static IDisposable SubscribeWith(
            this IAggregateEventSource aggregateEventSource,
            params IAggregateMaterializationStrategy[] aggregateMaterializationStrategies)
        {
            return aggregateEventSource.Subscribe(
                x =>
                {
                    foreach (var aggregateMaterializationStrategy in aggregateMaterializationStrategies)
                    {
                        aggregateMaterializationStrategy.HandleEvent(x);
                    }
                });
        }

        public static IDisposable SubscribeWith(this IEventSource eventSource, params IMaterializationStrategy[] materializationStrategies)
        {
            return eventSource.Subscribe(
                x =>
                {
                    foreach (var materializationStrategy in materializationStrategies)
                    {
                        materializationStrategy.HandleEvent(x);
                    }
                });
        }
    }
}
