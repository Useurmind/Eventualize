using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.Persistence;
using Eventualize.Persistence.Snapshots;

namespace Eventualize.EventStore.Persistence.SnapShots
{
    public class EventStoreSnapShotStore : ISnapShotStore
    {
        private const string SnapShotTypeMetaDataKey = "SnapShotTypeName";

        private IEventStoreConnection connection;

        private ISnapshotConverter snapshotConverter;

        public EventStoreSnapShotStore(IEventStoreConnection eventStoreConnection, ISnapshotConverter snapshotConverter)
        {
            this.connection = eventStoreConnection;
            this.snapshotConverter = snapshotConverter;
        }

        public IMemento GetSnapshot(AggregateIdentity aggregateIdentity)
        {
            var streamId = SnapShotStreamName.FromAggregateIdentity(aggregateIdentity);
            var resultSlice = this.connection.ReadStreamEventsBackwardAsync(streamId.ToString(), StreamPosition.End, 1, true).Result;

            if (!resultSlice.Events.Any())
            {
                // no snapshot saved
                return null;
            }

            var metaData = this.connection.GetStreamMetadataAsync(streamId.ToString()).Result;
            string snapShotTypeName = null;
            if (!metaData.StreamMetadata.TryGetValue(SnapShotTypeMetaDataKey, out snapShotTypeName))
            {
                // invalid stream, missing metadata for snapshot type
                return null;
            }

            var snapshotData = resultSlice.Events.Last().Event.Data;
            return this.snapshotConverter.BuildSnapshot(snapShotTypeName, snapshotData);
        }

        public void SaveSnapshot(AggregateIdentity aggregateIdentity, IMemento memento)
        {
            var streamId = SnapShotStreamName.FromAggregateIdentity(aggregateIdentity);
            var snapShotTypeName = memento.GetType().FullName;

            var metaData = StreamMetadata.Build()
                .SetCustomProperty(SnapShotTypeMetaDataKey, snapShotTypeName)
                .SetMaxCount(3)
                .Build();
            var snapshotData = this.snapshotConverter.GetSnapshotData(memento);
            var eventData = new EventData(Guid.NewGuid(), "Snapshot", true, snapshotData, null);

            var metaResult = this.connection.SetStreamMetadataAsync(streamId.ToString(), ExpectedVersion.Any, metaData).Result;
            var eventResult = this.connection.AppendToStreamAsync(streamId.ToString(), ExpectedVersion.Any, eventData).Result;
        }
    }
}
