using System;
using System.Linq;

namespace Eventualize.Materialization
{
    public interface IMaterializationEventPoller : IDisposable
    {
        void Run();
    }
}