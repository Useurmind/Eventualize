using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;

namespace Eventualize.Security
{
    public class EventualizeContext : IEventualizeContext
    {
        public EventualizeUser CurrentUser { get; set; }

        public EventNamespace DefaultEventNamespace { get; set; }

        public static void Init(UserId userId, EventNamespace defaultEventNamespace)
        {
            Current = new EventualizeContext()
            {
                CurrentUser = new EventualizeUser(userId),
                DefaultEventNamespace = defaultEventNamespace
            };
        }

        public static IEventualizeContext Current { get; private set; }

        public static EventNamespace TakeThisOrDefault(EventNamespace? eventNamespace)
        {
            return eventNamespace.HasValue ? eventNamespace.Value : Current.DefaultEventNamespace;
        }
    }
}
