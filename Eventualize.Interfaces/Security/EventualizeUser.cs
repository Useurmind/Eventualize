using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Security
{
    public class EventualizeUser
    {
        public EventualizeUser(UserId userId)
        {
            this.UserId = userId;
        }

        public UserId UserId { get; }
    }
}