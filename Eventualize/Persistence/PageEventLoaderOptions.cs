using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Persistence
{
    public class PageEventLoaderOptions
    {
        public int PageSize { get; set; }

        public AggregateVersion StartVersionEvent { get; set; }

        public AggregateVersion EndVersionEvent { get; set; }
    }
}