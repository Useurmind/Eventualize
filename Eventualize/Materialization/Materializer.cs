using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;
using Eventualize.Persistence;

using NEventStore;
using NEventStore.Client;

namespace Eventualize.Materialization
{
    public interface IMaterializationStrategy
    {
        void HandleEvent(Func<IAggregate> createAggregate, Guid aggregateId, object @event);
    }

    public class InMemoryMaterialization : IMaterializationStrategy
    {
        private ConcurrentDictionary<Guid, IAggregate> aggregates = new ConcurrentDictionary<Guid, IAggregate>();

        public IEnumerable<TAggregate> GetAggregates<TAggregate>() where TAggregate : IAggregate
        {
            return this.aggregates.Values.OfType<TAggregate>();
        }

        public void HandleEvent(Func<IAggregate> createAggregate, Guid aggregateId, object @event)
        {
            IAggregate aggregate = null;
            if (!this.aggregates.TryGetValue(aggregateId, out aggregate))
            {
                aggregate = createAggregate();
                this.aggregates[aggregate.Id] = aggregate;
            }

            aggregate.ApplyEvent(@event);
        }
    }

    public class EventStoreMaterializer
    {
        private IStoreEvents eventStore;

        private IMaterializationStrategy materializationStrategy;

        private IConstructAggregates aggregateFactory;

        private PollingClient pollingClient;

        private IDisposable subscription;

        private IObserveCommits observeCommits;

        private Assembly domainAssembly;

        public EventStoreMaterializer(IConstructAggregates aggregateFactory, IStoreEvents eventStore, IMaterializationStrategy materializationStrategy, Assembly domainAssembly)
        {
            this.aggregateFactory = aggregateFactory;
            this.eventStore = eventStore;
            this.materializationStrategy = materializationStrategy;
            this.domainAssembly = domainAssembly;
        }

        public void Run()
        {
            this.pollingClient = new PollingClient(this.eventStore.Advanced, 100);
            this.observeCommits = this.pollingClient.ObserveFrom();
            this.subscription = this.observeCommits.Subscribe(
                         commit =>
                             {
                                 var aggregateId = commit.StreamId.ToGuid();
                                 var events = commit.Events.Select(x => x.Body);
                                 var aggregateType = this.domainAssembly.GetType(commit.Headers["AggregateType"].ToString());

                                 foreach (var @event in events)
                                 {
                                     this.materializationStrategy.HandleEvent(() => this.aggregateFactory.Build(aggregateType, aggregateId, null), aggregateId, @event);
                                 }
                             });

            this.observeCommits.Start();
        }

    }
}