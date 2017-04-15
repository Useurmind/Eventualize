using System.Linq;

namespace Eventualize.Interfaces.Infrastructure
{
    public interface IEventualizeLogger
    {
        void Trace(string message);
    }
}