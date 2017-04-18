using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;
using Eventualize.Domain.MetaModel;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;
using Eventualize.Interfaces.Persistence;
using Eventualize.Persistence;
using Eventualize.Security;
using Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Eventualize.Test.Persistence
{
    public class InMemoryAggregateEventStoreTest
    {
        [Fact]
        public void GetEventsReturnsEmptyListWhenStreamIsEmpty()
        {
            var events = CreateEvents();
            var aggregateIdentity = CreateAggregateIdentity();

            var store = CreateStore();

            var foundEvents = store.GetEvents(aggregateIdentity, AggregateVersion.Start() + 1, AggregateVersion.Latest());

            foundEvents.Should().BeEmpty();
        }

        [Fact]
        public void GetEventsReturnsEmptyListWhenStartVersionIsToBig()
        {
            var events = CreateEvents();
            var aggregateIdentity = CreateAggregateIdentity();

            var store = CreateStore();

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());

            var foundEvents = store.GetEvents(aggregateIdentity, AggregateVersion.Start() + 1, AggregateVersion.Latest());

            foundEvents.Should().BeEmpty();
        }

        [Fact]
        public void AppendEventsSucceedsWhenAddingEventToNotCreatedStream()
        {
            var events = CreateEvents();
            var aggregateIdentity = CreateAggregateIdentity();

            var store = CreateStore();

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());

            var foundEvents = store.GetEvents(aggregateIdentity, AggregateVersion.Start(), AggregateVersion.Latest());

            foundEvents.Select(x => x.EventData).Should().BeEquivalentTo(events);
        }

        [Fact]
        public void AppendEventsThrowsExpectedAggregateVersionExceptionWhenExpectingEventsInEmptyStream()
        {
            var events = CreateEvents();
            var aggregateIdentity = CreateAggregateIdentity();

            var store = CreateStore();

            Assert.Throws<ExpectedAggregateVersionException>(
                () =>
                    {
                        store.AppendEvents(aggregateIdentity, new AggregateVersion(3), events, Guid.NewGuid());
                    });
        }

        [Fact]
        public void AppendEventsThrowsExpectedAggregateVersionExceptionWhenExpectingWrongVersion()
        {
            var events = CreateEvents();
            var aggregateIdentity = CreateAggregateIdentity();

            var store = CreateStore();

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
            var events = CreateEvents();
            var aggregateIdentity = CreateAggregateIdentity();

            var store = CreateStore();

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());
            Assert.Throws<ExpectedAggregateVersionException>(
                () =>
                {
                    store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), events, Guid.NewGuid());
                });
        }

        private static IEnumerable<IEventData> CreateEvents(int number = 1)
        {
            return Enumerable.Range(0, number).Select(x => new MyFirstEvent()).ToArray();
        }

        private static AggregateIdentity CreateAggregateIdentity()
        {
            return GetDomainIdentityProvider().GetAggregateIdentity(new MyFirstAggregate());
        }

        private static InMemoryAggregateEventStore CreateStore()
        {
            return new InMemoryAggregateEventStore(GetDomainIdentityProvider());
        }

        private static DomainModelIdentityProvider GetDomainIdentityProvider()
        {
            return new DomainModelIdentityProvider(GetDomainModel());
        }

        private static IDomainMetaModel GetDomainModel()
        {
            EventualizeContext.Init(new UserId("MyUser"));
            var domainModel = new ReflectionBasedMetaModelFactory(new[] { Assembly.GetExecutingAssembly() }).Build();
            return domainModel;
        }
    }
}
