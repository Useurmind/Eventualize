using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain;

namespace Eventualize.Persistence.Snapshots
{
    public class SnapshotConverter : ISnapshotConverter
    {

        private TypeRegister snapshotTypeRegister;

        private ISerializer serializer;
        public SnapshotConverter(ISerializer serializer)
        {
            this.serializer = serializer;
            this.snapshotTypeRegister = new TypeRegister();
        }

        public void ScanSnapshotTypes(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                this.ScanSnapshotTypes(assembly);
            }
        }

        public void ScanSnapshotTypes(Assembly assembly)
        {
            this.snapshotTypeRegister.ScanTypes(assembly, x => x.GetInterfaces().FirstOrDefault(y => y == typeof(IMemento)) != null, x => x.FullName);
        }
        public byte[] GetSnapshotData(IMemento memento)
        {
            return this.serializer.Serialize(memento);
        }

        public IMemento BuildSnapshot(string snapshotTypeName, byte[] snapShotData)
        {
            var snapShotType = this.snapshotTypeRegister.GetType(snapshotTypeName, () => $"Could not find type for snapshot of type {snapshotTypeName}.");

            var snapshot = (IMemento)this.serializer.Deserialize(snapShotType, snapShotData);
            return snapshot;
        }
    }
}