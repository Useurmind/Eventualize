using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Snapshots;

using FluentAssertions;

using Xunit;

namespace Eventualize.Test.Domain.Aggregates
{
    /// <summary>
    /// We test here with the <see cref="StateBackedAggregateBase{TState}"/> because aggregate base should not really be derived from.
    /// </summary>
    public class AggregateBaseTest
    {
        private class TestAggregateState : ISnapShot
        {
            public Guid Id { get; set; }
            public long Version { get; set; }

            public int Count { get; set; }
        }

        private class TestEvent1_IncreasesCount : IEventData
        {

        }

        private class TestAggregate : StateBackedAggregateBase<TestAggregateState>
        {
            public int Count
            {
                get
                {
                    return this.State.Count;
                }
            }

            public void ExecuteCommandForTestEvent1()
            {
                this.RaiseEvent(new TestEvent1_IncreasesCount());
            }

            private void Apply(TestEvent1_IncreasesCount @event)
            {
                this.State.Count++;
            }
        }

        [Fact]
        public void StartingVersionIsNotCreatedAggregateVersion()
        {
            var aggregate = new TestAggregate();

            aggregate.Version.Should().Be(AggregateVersion.NotCreated);
        }

        [Fact]
        public void ClearingUncommitedEventsWorks()
        {
            var aggregate = new TestAggregate();

            aggregate.ExecuteCommandForTestEvent1();
            aggregate.ExecuteCommandForTestEvent1();

            aggregate.As<IAggregate>().ClearUncommittedEvents();

            aggregate.Version.Should().Be(1);
            aggregate.As<IAggregate>().CommittedVersion.Should().Be(1);
            aggregate.As<IAggregate>().GetUncommittedEvents().Count.Should().Be(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(100)]
        public void ApplyingEventFromStoreIncreasesCount(int numberEventsApplied)
        {
            var aggregate = GetAggregateWithNumberOfEvents(numberEventsApplied);

            aggregate.Count.Should().Be(numberEventsApplied);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(100)]
        public void ApplyingEventFromStoreCorrectlyIncreasesVersion(int numberEventsApplied)
        {
            var aggregate = GetAggregateWithNumberOfEvents(numberEventsApplied);

            aggregate.As<IAggregate>().Version.Should().Be(numberEventsApplied - 1);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(100)]
        public void ApplyingEventFromStoreLeadsToCorrectCommitedVersionAndUncommitedEvents(int numberEventsApplied)
        {
            var aggregate = GetAggregateWithNumberOfEvents(numberEventsApplied);

            aggregate.As<IAggregate>().CommittedVersion.Should().Be(numberEventsApplied - 1);
            aggregate.As<IAggregate>().GetUncommittedEvents().Any().Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(100)]
        public void ApplyingEventFromStoreGivesCorrectSnapshot(int numberEventsApplied)
        {
            var aggregate = GetAggregateWithNumberOfEvents(numberEventsApplied);

            var snapshot = aggregate.As<IAggregate>().GetSnapshot();

            snapshot.As<TestAggregateState>().Id.Should().Be(aggregate.Id);
            snapshot.As<TestAggregateState>().Version.Should().Be(numberEventsApplied - 1);
            snapshot.As<TestAggregateState>().Count.Should().Be(numberEventsApplied);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(100)]
        public void RaisingEventFromCommandLeadsToCorrectVersion(int numberEventsApplied)
        {
            var aggregate = GetAggregateWithNumberOfEvents(numberEventsApplied, false);

            aggregate.Version.Should().Be(numberEventsApplied - 1);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(100)]
        public void RaisingEventFromCommandLeadsToCorrectCommitedVersionAndEvents(int numberEventsApplied)
        {
            var aggregate = GetAggregateWithNumberOfEvents(numberEventsApplied, false);

            aggregate.As<IAggregate>().CommittedVersion.Should().Be(AggregateVersion.NotCreated);
            aggregate.As<IAggregate>().GetUncommittedEvents().Count.Should().Be(numberEventsApplied);
        }

        [Fact]
        public void ApplyingASnapshotWorksAsExpected()
        {
            var snapshot = new TestAggregateState()
            {
                Id = Guid.NewGuid(),
                Count = 3,
                Version = 2
            };

            var aggregate = new TestAggregate();
            aggregate.As<IAggregate>().ApplySnapshot(snapshot);

            aggregate.Id.Should().Be(snapshot.Id);
            aggregate.Version.Should().Be(snapshot.Version);
            aggregate.Count.Should().Be(snapshot.Count);
        }

        [Fact]
        public void AggregatesFromSameSnapshotAreEqual()
        {
            var snapshot = new TestAggregateState()
            {
                Id = Guid.NewGuid()
            };

            var aggregate1 = new TestAggregate();
            var aggregate2 = new TestAggregate();

            aggregate1.As<IAggregate>().ApplySnapshot(snapshot);
            aggregate2.As<IAggregate>().ApplySnapshot(snapshot);

            aggregate1.Equals(aggregate2).Should().BeTrue();
        }

        [Fact]
        public void MergingSnapshotAndEventsFromStoreWorks()
        {
            var snapshot = new TestAggregateState()
            {
                Id = Guid.NewGuid(),
                Version = 3,
                Count = 4
            };

            var aggregate = new TestAggregate();
            aggregate.As<IAggregate>().ApplySnapshot(snapshot);
            aggregate.As<IAggregate>().ApplyEvent(new TestEvent1_IncreasesCount());
            aggregate.As<IAggregate>().ApplyEvent(new TestEvent1_IncreasesCount());

            aggregate.Id.Should().Be(snapshot.Id);
            aggregate.Version.Should().Be(5);
            aggregate.Count.Should().Be(6);
            aggregate.CommittedVersion.Should().Be(aggregate.Version);
            aggregate.As<IAggregate>().GetUncommittedEvents().Any().Should().BeFalse();
        }

        [Fact]
        public void MergingSnapshotAndEventsFromStoreAndCommandsWorks()
        {
            var snapshot = new TestAggregateState()
            {
                Id = Guid.NewGuid(),
                Version = 3,
                Count = 4
            };

            var aggregate = new TestAggregate();
            aggregate.As<IAggregate>().ApplySnapshot(snapshot);
            aggregate.As<IAggregate>().ApplyEvent(new TestEvent1_IncreasesCount());
            aggregate.As<IAggregate>().ApplyEvent(new TestEvent1_IncreasesCount());
            aggregate.ExecuteCommandForTestEvent1();
            aggregate.ExecuteCommandForTestEvent1();
            aggregate.ExecuteCommandForTestEvent1();

            aggregate.Id.Should().Be(snapshot.Id);
            aggregate.Version.Should().Be(8);
            aggregate.Count.Should().Be(9);
            aggregate.CommittedVersion.Should().Be(aggregate.Version-3);
            aggregate.As<IAggregate>().GetUncommittedEvents().Count.Should().Be(3);
        }

        private static TestAggregate GetAggregateWithNumberOfEvents(int numberEventsApplied, bool fromStore = true)
        {
            var aggregate = new TestAggregate();

            for (int index = 0; index < numberEventsApplied; index++)
            {
                if (fromStore)
                {
                    ((IAggregate)aggregate).ApplyEvent(new TestEvent1_IncreasesCount());
                }
                else
                {
                    aggregate.ExecuteCommandForTestEvent1();
                }
            }
            return aggregate;
        }
    }
}
