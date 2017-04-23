using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Persistence;
using Eventualize.Test.Persistence;
using Eventualize.Test.TestDomain;
using Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates;
using Eventualize.Test.TestDomain.FirstContext.MySecondAggregates;
using Eventualize.Test.TestDomain.SecondContext.MyThirdAggregate;

using FluentAssertions;

using Xunit;

namespace Eventualize.Test.ReactiveStreams
{
    public class InMemoryStoreTest
    {
        [Fact]
        public void AnEventAppendedToTheStoreIsForwardedToAllSubscription()
        {
            var eventList = new List<IEvent>();

            var container = new TestContainer();
            var store = container.CreateStore();
            var aggregateIdentity = container.CreateAggregateIdentity<MyFirstAggregate>();

            var factory = new InMemoryEventSourceFactory(store);

            factory.FromAll().Subscribe(x => eventList.Add(x));

            store.AppendEvents(aggregateIdentity, AggregateVersion.NotCreated(), new []{new MyFirstEvent() }, Guid.NewGuid());

            eventList.Count.Should().Be(1);
            eventList.First().EventData.Should().BeOfType<MyFirstEvent>();
        }

        [Fact]
        public void EventsForDifferentBoundedContextsAreFilteredInBoundedContextSubscription()
        {
            var eventList = new List<IEvent>();

            var container = new TestContainer();
            var store = container.CreateStore();
            var aggregateIdentity1 = container.CreateAggregateIdentity<MyFirstAggregate>();
            var aggregateIdentity2 = container.CreateAggregateIdentity<MyThirdAggregate>();

            var factory = new InMemoryEventSourceFactory(store);

            factory.FromBoundedContext(new BoundedContextName(DomainNames.FirstContextName)).Subscribe(x => eventList.Add(x));

            store.AppendEvents(aggregateIdentity1, AggregateVersion.NotCreated(), new[] { new MyFirstEvent() }, Guid.NewGuid());
            store.AppendEvents(aggregateIdentity2, AggregateVersion.NotCreated(), new[] { new MyThirdEvent(),  }, Guid.NewGuid());

            eventList.Count.Should().Be(1);
            eventList.First().EventData.Should().BeOfType<MyFirstEvent>();
        }

        [Fact]
        public void EventsForDifferentAggregateTypesAreFilteredInAggregateTypeSubscription()
        {
            var eventList = new List<IEvent>();

            var container = new TestContainer();
            var store = container.CreateStore();
            var aggregateIdentity1 = container.CreateAggregateIdentity<MyFirstAggregate>();
            var aggregateIdentity2 = container.CreateAggregateIdentity<MySecondAggregate>();

            var factory = new InMemoryEventSourceFactory(store);

            factory.FromAggregateType(new BoundedContextName(DomainNames.FirstContextName), aggregateIdentity1.AggregateTypeName).Subscribe(x => eventList.Add(x));

            store.AppendEvents(aggregateIdentity1, AggregateVersion.NotCreated(), new[] { new MyFirstEvent() }, Guid.NewGuid());
            store.AppendEvents(aggregateIdentity2, AggregateVersion.NotCreated(), new[] { new MySecondEvent(), }, Guid.NewGuid());

            eventList.Count.Should().Be(1);
            eventList.First().EventData.Should().BeOfType<MyFirstEvent>();
        }

        [Fact]
        public void FilteringEventStreamsWorks()
        {
            var eventList = new List<IEvent>();

            var container = new TestContainer();
            var store = container.CreateStore();
            var aggregateIdentity1 = container.CreateAggregateIdentity<MyFirstAggregate>();

            var factory = new InMemoryEventSourceFactory(store);

            factory.FromAggregateType(new BoundedContextName(DomainNames.FirstContextName), aggregateIdentity1.AggregateTypeName)
                .Where(x => x.StoreIndex < 2)
                .Subscribe(x => eventList.Add(x));

            store.AppendEvents(aggregateIdentity1, AggregateVersion.NotCreated(), new[] { new MyFirstEvent() }, Guid.NewGuid());
            store.AppendEvents(aggregateIdentity1, new AggregateVersion(0), new[] { new MyFirstEvent() }, Guid.NewGuid());
            store.AppendEvents(aggregateIdentity1, new AggregateVersion(1), new[] { new MyFirstEvent() }, Guid.NewGuid());

            eventList.Count.Should().Be(2);
            eventList.Select(x => x.EventData).Should().AllBeOfType<MyFirstEvent>();
        }
    }
}
