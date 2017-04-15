using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Security;

namespace Eventualize.Security
{
    public class EventualizeContext : IEventualizeContext
    {
        public EventualizeUser CurrentUser { get; set; }

        public BoundedContext DefaultBoundedContext { get; set; }

        public static void Init(UserId userId, BoundedContext defaultBoundedContext)
        {
            Current = new EventualizeContext()
            {
                CurrentUser = new EventualizeUser(userId),
                DefaultBoundedContext = defaultBoundedContext
            };
        }

        public static IEventualizeContext Current { get; private set; }

        public static BoundedContext TakeThisOrDefault(BoundedContext? boundedContext)
        {
            return boundedContext.HasValue ? boundedContext.Value : Current.DefaultBoundedContext;
        }
    }
}
