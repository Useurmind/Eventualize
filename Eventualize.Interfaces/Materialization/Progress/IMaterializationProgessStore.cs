using System.Linq;
using System.Threading.Tasks;

namespace Eventualize.Interfaces.Materialization.Progress
{
    public interface IMaterializationProgessStore
    {
        T GetProgess<T>(string key);

        Task<T> GetProgessAsync<T>(string key);

        void SaveProgess<T>(string key, T currentProgess);
    }
}