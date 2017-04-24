using System.Linq;
using System.Threading.Tasks;

using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Progress;

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

        public async Task<T> GetAsync<T>()
        {
            return await this.store.GetProgessAsync<T>(this.key);
        }

        public void Set<T>(T currentProgess)
        {
            this.store.SaveProgess(this.key, currentProgess);
        }
    }
}