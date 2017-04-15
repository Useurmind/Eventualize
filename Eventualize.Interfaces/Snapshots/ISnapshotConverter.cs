using System.Linq;

namespace Eventualize.Interfaces.Snapshots
{
    public interface ISnapshotConverter
    {
        byte[] GetSnapshotData(ISnapShot snapShot);

        ISnapShot BuildSnapshot(string snapshotTypeName, byte[] snapShotData);
    }
}