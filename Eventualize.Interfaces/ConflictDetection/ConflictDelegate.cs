using System.Linq;

namespace Eventualize.Interfaces.ConflictDetection
{
    public delegate bool ConflictDelegate(object uncommitted, object committed);

    public delegate bool ConflictDelegate<in TUncommitted, in TCommitted>(TUncommitted uncommitted, TCommitted committed)
       where TUncommitted : class
       where TCommitted : class;
}