using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Snapshots
{
    /// <summary>
    /// An interface to load and save snapshots for aggregates.
    /// </summary>
    public interface ISnapShotStore
    {
        /// <summary>
        /// Get a snapshot for an aggregate.
        /// </summary>
        /// <param name="aggregateIdentity">The identity of the aggregate for which the snapshot should be retrieved.</param>
        /// <param name="forVersion">The version of the aggregate that should be recreated from the snapshot.</param>
        /// <returns>A matching snapshot or null</returns>
        ISnapShot GetSnapshot(AggregateIdentity aggregateIdentity, AggregateVersion forVersion);

        /// <summary>
        /// Save a snapshot for an aggregate.
        /// </summary>
        /// <param name="aggregateIdentity">The identity of the aggregate for which the snapshot should be stored.</param>
        /// <param name="snapShot">The snapshot to store.</param>
        void SaveSnapshot(AggregateIdentity aggregateIdentity, ISnapShot snapShot);
    }
}
