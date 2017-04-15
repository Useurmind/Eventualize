using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Snapshots
{
    public interface ISnapShotStore
    {
        ISnapShot GetSnapshot(AggregateIdentity aggregateIdentity);

        void SaveSnapshot(AggregateIdentity aggregateIdentity, ISnapShot snapShot);
    }
}
