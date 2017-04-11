using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eventualize.Security
{
    public class EventualizeContext : IEventualizeContext
    {
        public EventualizeUser CurrentUser { get; set; }

        public static void Init(string userId)
        {
            Current = new EventualizeContext()
            {
                CurrentUser = new EventualizeUser(userId)
            };
        }

        public static IEventualizeContext Current { get; private set; }
    }
}
