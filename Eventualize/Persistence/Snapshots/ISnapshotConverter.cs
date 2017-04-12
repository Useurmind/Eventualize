using Eventualize.Domain;

namespace Eventualize.Persistence.Snapshots
{
    public interface ISnapshotConverter
    {
        byte[] GetSnapshotData(IMemento memento);

        IMemento BuildSnapshot(string snapshotTypeName, byte[] snapShotData);
    }
}