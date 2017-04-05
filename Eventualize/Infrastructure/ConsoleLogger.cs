using System;
using System.Linq;

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