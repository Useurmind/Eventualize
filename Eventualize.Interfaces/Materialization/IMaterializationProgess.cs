using System.Linq;

namespace Eventualize.Interfaces.Materialization
{
    public interface IMaterializationProgess
    {
        T Get<T>();

        void Set<T>(T currentProgess);
    }
}
