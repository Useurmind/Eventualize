using System.Linq;

namespace Eventualize.Infrastructure
{
    public interface IEventualizeLogger
    {
        void Trace(string message);
    }
}