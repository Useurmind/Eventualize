using System.Linq;

namespace Eventualize.Security
{
    public class EventualizeUser
    {
        public EventualizeUser(string userId)
        {
            this.UserId = userId;
        }

        public string UserId { get; }
    }
}