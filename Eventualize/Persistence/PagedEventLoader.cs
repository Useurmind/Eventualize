using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Persistence;

namespace Eventualize.Persistence
{
    public class PagedEventLoader : IPagedEventLoader
    {
        public void LoadAllPages(IAggregateEventStore eventStore, AggregateIdentity aggregateIdentity, PageEventLoaderOptions options, Action<IAggregateEvent> eventAction)
        {
            var currentPageStart = options.StartVersionEvent;
            var currentPageEnd = GetEndEventNumber(options.EndVersionEvent);
            RestrictPageSize(currentPageStart, ref currentPageEnd, options.PageSize);

            while (currentPageEnd >= currentPageStart)
            {
                var events = eventStore.GetEvents(aggregateIdentity, currentPageStart, currentPageEnd);
                
                foreach (var @event in events)
                {
                    eventAction(@event);
                }

                currentPageStart = currentPageEnd + 1;
                currentPageEnd = GetEndEventNumber(options.EndVersionEvent);
                RestrictPageSize(currentPageStart, ref currentPageEnd, options.PageSize);
            }

        }

        /// <summary>
        /// Get the last event number that should be retrieved.
        /// </summary>
        /// <param name="endEventNumber">The target end event number (which can be AggregateVersion.Latest)</param>
        /// <returns></returns>
        private static AggregateVersion GetEndEventNumber(AggregateVersion endEventNumber)
        {
            return endEventNumber == AggregateVersion.Latest()
                       ? new AggregateVersion(long.MaxValue)
                       : endEventNumber;
        }

        /// <summary>
        /// Restrict the page to the max page size.
        /// </summary>
        /// <param name="currentPageStart"></param>
        /// <param name="currentPageEnd"></param>
        /// <param name="maxPageSize"></param>
        private static void RestrictPageSize(AggregateVersion currentPageStart, ref AggregateVersion currentPageEnd, int maxPageSize)
        {
            if (currentPageEnd - currentPageStart > maxPageSize)
            {
                currentPageEnd = currentPageStart + maxPageSize;
            }
        }
    }
}
