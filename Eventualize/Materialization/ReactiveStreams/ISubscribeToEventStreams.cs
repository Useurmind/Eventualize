using System.Linq;
using System.Threading.Tasks;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Materialization.ReactiveStreams
{
    /// <summary>
    /// This is an interface for an event subscriber.
    /// </summary>
    public interface ISubscribeToEventStreams
    {
        /// <summary>
        /// Get the index of the last handled event (during the last run) or null (if it should start from scratch).
        /// </summary>
        /// <returns></returns>
        Task<EventStreamIndex?> GetLastHandledEventIndexAsync();

        /// <summary>
        /// Perform any subscription logic to register event handlers.
        /// </summary>
        /// <param name="eventSourceProvider">This is a provider for different kinds of event streams.</param>
        void SubscribeStreams(IEventSourceProvider eventSourceProvider);
    }
}