using System;
using System.Linq;

namespace Eventualize.Materialization
{
    public interface IMaterializer : IDisposable
    {
        void Run();
    }
}