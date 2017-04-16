using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

namespace Eventualize.EventStore.Persistence
{
    public class PageEventLoaderOptions
    {
        public int PageSize { get; set; }
    }

    public interface IPagedEventLoader
    {
        void LoadAllPages(Action<ResolvedEvent> eventAction, PageEventLoaderOptions options);
    }

    public class PagedEventLoader : IPagedEventLoader
    {
        public void LoadAllPages(Action<ResolvedEvent> eventAction, PageEventLoaderOptions options)
        {
            
        }
    }
}
