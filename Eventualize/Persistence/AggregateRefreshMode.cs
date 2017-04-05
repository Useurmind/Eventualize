using System.Linq;

namespace Eventualize.Persistence
{
    public enum AggregateRefreshMode
    {
        FailIfUncommittedChanges,
        ReapplyUncommittedAfterRefresh
    }
}