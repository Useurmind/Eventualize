using System.Linq;
using System.Threading.Tasks;

namespace Eventualize.Interfaces.Materialization.Progress
{
    public interface IMaterializationProgess
    {
        T Get<T>();

        Task<T> GetAsync<T>();

        void Set<T>(T currentProgess);
    }
}
