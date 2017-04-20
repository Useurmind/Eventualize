using System.Linq;

namespace Eventualize.Interfaces.Materialization.Progress
{
    public interface IMaterializationProgessStore
    {
        T GetProgess<T>(string key);

        void SaveProgess<T>(string key, T currentProgess);
    }
}