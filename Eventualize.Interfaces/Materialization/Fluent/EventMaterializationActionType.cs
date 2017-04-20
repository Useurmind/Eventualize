using System.Linq;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public enum EventMaterializationActionType
    {
        Merge,
        Insert,
        Update,
        Delete
    }
}