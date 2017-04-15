using System.Linq;

namespace Eventualize.Interfaces.Infrastructure
{
    // TODO
    public interface ICache
    {
        void Put(object key, object data);

        bool TryGet(object key, out object data);
    }

    //public class MemoryCacheImpl : ICache
    //{
    //    private MemoryCache cache;

    //    public MemoryCacheImpl( )
    //    {
    //        this.cache = new MemoryCache(Guid.NewGuid().ToString());
    //    }

    //    public void Put(string key, object data)
    //    {
    //        this.cache.Add(new CacheItem(key, data), new CacheItemPolicy()
    //                                                 {
                                                         
    //                                                 });
    //    }

    //    public bool TryGet(string key, out object data)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
