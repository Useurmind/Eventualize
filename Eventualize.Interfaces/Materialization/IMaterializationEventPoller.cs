using System;
using System.Linq;

namespace Eventualize.Interfaces.Materialization
{
    public interface IMaterializationEventPoller : IDisposable
    {
        void Run();
    }
}