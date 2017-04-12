using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Security
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