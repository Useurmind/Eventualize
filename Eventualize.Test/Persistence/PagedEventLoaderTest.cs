using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Persistence;
using Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates;

using FluentAssertions;

using Xunit;

namespace Eventualize.Test.Persistence
{
    public class PagedEventLoaderTest
    {
        private TestContainer container;

        public PagedEventLoaderTest( )
        {
            this.container = new TestContainer();
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(3, 10)]
        [InlineData(15, 10)]
        [InlineData(25, 10)]
        [InlineData(25, 1024)]
        [InlineData(2500, 1024)]
        public void RequestingEventsWithStartToLatestWorks(int numberEvents, int pageSize)
        {
            var retrievedEvents = new List<IAggregateEvent>();
            var pageLoader = new PagedEventLoader();
            var store = this.container.CreateStore();
            var aggregateIdentity = this.container.CreateAggregateIdentity<MyFirstAggregate>();
            var events = this.container.CreateEvents<MyFirstEvent>(numberEvents);
            var options = new PageEventLoaderOptions()
                          {
                              PageSize = pageSize,
                              StartVersionEvent = AggregateVersion.Start(),
                              EndVersionEvent = AggregateVersion.Latest()
                          };

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());

            pageLoader.LoadAllPages(store, aggregateIdentity, options, aggEvent => retrievedEvents.Add(aggEvent));

            retrievedEvents.Select(x => x.EventData).ShouldBeEquivalentTo(events);
        }

        [Theory]
        [InlineData(2, 10, 1, 1)]
        [InlineData(15, 10, 3, 12)]
        [InlineData(15, 10, 3, 14)]
        [InlineData(25, 10, 3, 24)]
        [InlineData(26, 10, 14, 25)]
        public void RequestingEventsWithSpecificVersionsWorks(int numberEvents, int pageSize, int startVersion, int endVersion)
        {
            var retrievedEvents = new List<IAggregateEvent>();
            var pageLoader = new PagedEventLoader();
            var store = this.container.CreateStore();
            var aggregateIdentity = this.container.CreateAggregateIdentity<MyFirstAggregate>();
            var events = this.container.CreateEvents<MyFirstEvent>(numberEvents);
            var options = new PageEventLoaderOptions()
            {
                PageSize = pageSize,
                StartVersionEvent = new AggregateVersion(startVersion),
                EndVersionEvent = new AggregateVersion(endVersion)
            };

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());

            pageLoader.LoadAllPages(store, aggregateIdentity, options, aggEvent => retrievedEvents.Add(aggEvent));

            retrievedEvents.Select(x => x.EventData).ShouldBeEquivalentTo(events.Skip(startVersion).Take(endVersion-startVersion+1));
        }
    }
}
