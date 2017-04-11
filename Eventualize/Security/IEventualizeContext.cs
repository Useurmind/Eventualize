using System.Linq;

namespace Eventualize.Security
{
    public interface IEventualizeContext
    {
        EventualizeUser CurrentUser { get; }
    }
}