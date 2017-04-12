using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Security
{
    public interface IEventualizeContext
    {
        EventualizeUser CurrentUser { get; }

        EventNamespace DefaultEventNamespace { get; }
    }
}