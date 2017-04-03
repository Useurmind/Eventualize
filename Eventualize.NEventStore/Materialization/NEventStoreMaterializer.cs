using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Materialization;
using Eventualize.Persistence;
using Eventualize.Domain;

using NEventStore;
using NEventStore.Client;

namespace Eventualize.NEventStore.Materialization
{
    public class NEventStoreMaterializer : IMaterializer
    {
        private IStoreEvents eventStore;

        private IEnumerable<IMaterializationStrategy> materializationStrategies;

        private IConstructInstances aggregateFactory;

        private PollingClient pollingClient;

        private IDisposable subscription;

        private IObserveCommits observeCommits;

        private Assembly domainAssembly;

        public NEventStoreMaterializer(IConstructInstances aggregateFactory, IStoreEvents eventStore, IEnumerable<IMaterializationStrategy> materializationStrategies)
        {
            this.aggregateFactory = aggregateFactory;
            this.eventStore = eventStore;
            this.materializationStrategies = materializationStrategies;
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
                            foreach (var materializationStrategy in this.materializationStrategies)
                            {
                                materializationStrategy.HandleEvent(() => this.aggregateFactory.BuildAggregate(aggregateTypeName, aggregateId, null), aggregateId, @event);
                            }
                        }
                    });

            this.observeCommits.Start();
        }

        public void Dispose()
        {
            this.subscription.Dispose();
            this.observeCommits.Dispose();
        }
    }
}