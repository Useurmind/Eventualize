using System.Linq;

namespace Eventualize.Domain.Core
{
    public delegate bool ConflictDelegate(object uncommitted, object committed);
}