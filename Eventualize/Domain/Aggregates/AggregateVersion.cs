using System.Linq;

namespace Eventualize.Domain.Aggregates
{
    public static class AggregateVersion
    {
        public const int Latest = -2;
        public const int NotCreated = -1;
    }
}