using System;
using System.Linq;
using System.Reflection;

using Eventualize.Materialization;
using Eventualize.Persistence;
using Eventualize.Domain;

using NEventStore;
using NEventStore.Client;

namespace Eventualize.NEventStore.Materialization
{
    public class EventStoreMaterializer
    {
        private IStoreEvents eventStore;

        private IMaterializationStrategy materializationStrategy;

        private IConstructAggregates aggregateFactory;

        private PollingClient pollingClient;

        private IDisposable subscription;

        private IObserveCommits observeCommits;

        private Assembly domainAssembly;

        public EventStoreMaterializer(IConstructAggregates aggregateFactory, IStoreEvents eventStore, IMaterializationStrategy materializationStrategy)
        {
            this.aggregateFactory = aggregateFactory;
            this.eventStore = eventStore;
            this.materializationStrategy = materializationStrategy;
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
                        var aggregateTypeName = commit.Headers["AggregateType"].ToString();

                        foreach (var @event in events)
                        {
                            this.materializationStrategy.HandleEvent(() => this.aggregateFactory.Build(aggregateTypeName, aggregateId, null), aggregateId, @event);
                        }
                    });

            this.observeCommits.Start();
        }

    }
}