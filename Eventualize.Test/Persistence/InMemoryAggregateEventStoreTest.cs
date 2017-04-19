using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Persistence;
using Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Eventualize.Test.Persistence
{
    public class InMemoryAggregateEventStoreTest
    {
        private TestContainer testContainer;

        public InMemoryAggregateEventStoreTest()
        {
            this.testContainer = new TestContainer();
        }

        [Fact]
        public void GetEventsReturnsEmptyListWhenStreamIsEmpty()
        {
            var events = this.testContainer.CreateEvents<MyFirstEvent>();
            var aggregateIdentity = this.testContainer.CreateAggregateIdentity<MyFirstAggregate>();
            var store = this.testContainer.CreateStore();

            var foundEvents = store.GetEvents(aggregateIdentity, AggregateVersion.Start() + 1, AggregateVersion.Latest());

            foundEvents.Should().BeEmpty();
        }

        [Fact]
        public void GetEventsReturnsEmptyListWhenStartVersionIsToBig()
        {
            var events = this.testContainer.CreateEvents<MyFirstEvent>();
            var aggregateIdentity = this.testContainer.CreateAggregateIdentity<MyFirstAggregate>();
            var store = this.testContainer.CreateStore();

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());

            var foundEvents = store.GetEvents(aggregateIdentity, AggregateVersion.Start() + 1, AggregateVersion.Latest());

            foundEvents.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void AppendEventsSucceedsWhenAddingEventsToNotCreatedStream(int numberEvents)
        {
            var events = this.testContainer.CreateEvents<MyFirstEvent>(numberEvents);
            var aggregateIdentity = this.testContainer.CreateAggregateIdentity<MyFirstAggregate>();
            var store = this.testContainer.CreateStore();

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());

            var foundEvents = store.GetEvents(aggregateIdentity, AggregateVersion.Start(), AggregateVersion.Latest());

            foundEvents.Select(x => x.EventData).Should().BeEquivalentTo(events);
        }

        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(2, 0, 1)]
        [InlineData(10, 0, 5)]
        [InlineData(10, 3, 5)]
        [InlineData(10, 3, 15)]
        [InlineData(10, 12, 15)]
        public void GetEventsSucceedsWhenRetrievingWithSpecificVersions(int numberEvents, int startVersion, int endVersion)
        {
            var events = this.testContainer.CreateEvents<MyFirstEvent>(numberEvents);
            var aggregateIdentity = this.testContainer.CreateAggregateIdentity<MyFirstAggregate>();
            var store = this.testContainer.CreateStore();

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());

            var foundEvents = store.GetEvents(aggregateIdentity, new AggregateVersion(startVersion), new AggregateVersion(endVersion));

            foundEvents.Select(x => x.EventData).Should().BeEquivalentTo(events.Skip(startVersion).Take(endVersion - startVersion + 1));
        }

        [Fact]
        public void AppendEventsThrowsExpectedAggregateVersionExceptionWhenExpectingEventsInEmptyStream()
        {
            var events = this.testContainer.CreateEvents<MyFirstEvent>();
            var aggregateIdentity = this.testContainer.CreateAggregateIdentity<MyFirstAggregate>();
            var store = this.testContainer.CreateStore();

            Assert.Throws<ExpectedAggregateVersionException>(
                () =>
                {
                    store.AppendEvents(aggregateIdentity, new AggregateVersion(3), events, Guid.NewGuid());
                });
        }

        [Fact]
        public void AppendEventsThrowsExpectedAggregateVersionExceptionWhenExpectingWrongVersion()
        {
            var events = this.testContainer.CreateEvents<MyFirstEvent>();
            var aggregateIdentity = this.testContainer.CreateAggregateIdentity<MyFirstAggregate>();
            var store = this.testContainer.CreateStore();

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());
            Assert.Throws<ExpectedAggregateVersionException>(
                () =>
                {
                    store.AppendEvents(aggregateIdentity, new AggregateVersion(3), events, Guid.NewGuid());
                });
        }

        [Fact]
        public void AppendEventsThrowsExpectedAggregateVersionExceptionWhenExpectingThatStreamIsEmptyButIsNot()
        {
            var events = this.testContainer.CreateEvents<MyFirstEvent>();
            var aggregateIdentity = this.testContainer.CreateAggregateIdentity<MyFirstAggregate>();
            var store = this.testContainer.CreateStore();

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());
            Assert.Throws<ExpectedAggregateVersionException>(
                () =>
                {
                    store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());
                });
        }
    }
}
