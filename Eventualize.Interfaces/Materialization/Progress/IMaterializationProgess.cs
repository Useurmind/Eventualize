using System.Linq;

namespace Eventualize.Interfaces.Materialization.Progress
{
    public interface IMaterializationProgess
    {
        T Get<T>();

        void Set<T>(T currentProgess);
    }
}
