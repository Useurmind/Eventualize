using System;
using System.Linq;

using Eventualize.Interfaces.Infrastructure;

namespace Eventualize.Infrastructure
{
    public class ConsoleLogger : IEventualizeLogger
    {
        public void Trace(string message)
        {
            Console.WriteLine(message);
        }
    }
}