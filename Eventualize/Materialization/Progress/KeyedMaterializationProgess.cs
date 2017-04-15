using System.Linq;

using Eventualize.Interfaces.Materialization;

namespace Eventualize.Materialization.Progress
{
    public class KeyedMaterializationProgess : IMaterializationProgess
    {
        private IMaterializationProgessStore store;

        private string key;

        public KeyedMaterializationProgess(IMaterializationProgessStore store, string key)
        {
            this.store = store;
            this.key = key;
        }

        public T Get<T>()
        {
            return this.store.GetProgess<T>(this.key);
        }

        public void Set<T>(T currentProgess)
        {
            this.store.SaveProgess(this.key, currentProgess);
        }
    }
}