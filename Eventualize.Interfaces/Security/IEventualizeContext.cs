using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Security
{
    public interface IEventualizeContext
    {
        EventualizeUser CurrentUser { get; }
    }
}