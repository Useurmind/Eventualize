using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Materialization;
using Eventualize.Persistence;
using Eventualize.Domain;
using Eventualize.NEventStore.Persistence;

using NEventStore;
using NEventStore.Client;

namespace Eventualize.NEventStore.Materialization
{
    public class NEventStoreMaterializationEventPoller : IMaterializationEventPoller
    {
        private IStoreEvents eventStore;

        private IEnumerable<IMaterializationStrategy> materializationStrategies;

        private IConstructInstances aggregateFactory;

        private PollingClient pollingClient;

        private IDisposable subscription;

        private IObserveCommits observeCommits;

        private Assembly domainAssembly;

        public NEventStoreMaterializationEventPoller(IConstructInstances aggregateFactory, IStoreEvents eventStore, IEnumerable<IMaterializationStrategy> materializationStrategies)
        {
            this.aggregateFactory = aggregateFactory;
            this.eventStore = eventStore;
            this.materializationStrategies = materializationStrategies;
        }

        public void Run()
        {
            this.pollingClient = new PollingClient(this.eventStore.Advanced, 100);
            this.observeCommits = this.pollingClient.ObserveFromBucket(NEventStoreBuckets.Aggregates);
            this.subscription = this.observeCommits.Subscribe(
                commit =>
                    {
                        var aggregateId = commit.StreamId.ToGuid();
                        var aggregateTypeName = commit.Headers["AggregateType"].ToString();

                        var aggregateIdentity = new AggregateIdentity(
                            new EventNamespace(commit.BucketId),
                            new AggregateTypeName(aggregateTypeName),
                            aggregateId);

                        foreach (var @event in commit.Events)
                        {
                            var materializationEvent = NEventStoreEventConverter.CreateAggregateEvent(
                                aggregateIdentity,
                                commit.CommitId,
                                @event,
                                -1);

                            foreach (var materializationStrategy in this.materializationStrategies)
                            {
                                materializationStrategy.HandleEvent(materializationEvent);
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