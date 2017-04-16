using System.Linq;

namespace Eventualize.Persistence
{
    public class PageEventLoaderOptions
    {
        public int PageSize { get; set; }

        public long StartEventNumber { get; set; }

        public long EndEventNumber { get; set; }
    }
}