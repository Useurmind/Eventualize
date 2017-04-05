using System.Linq;

namespace Eventualize.Domain.Core
{
    public static class AggregateVersion
    {
        public const int Latest = -2;
        public const int NotCreated = -1;
    }
}