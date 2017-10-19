using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCache
{
    public class LRUCachePolicy<K, V> : ICachePolicy<K, V>
    {
        LinkedList<CacheItem<K, V>> queue;

        public LRUCachePolicy()
        {
            this.queue  = new LinkedList<CacheItem<K, V>>();
        }

        public void OnAdd(CacheItem<K, V> cacheItem)
        {
            queue.AddFirst(cacheItem);
        }

        public K EvictItem()
        {
            CacheItem<K, V> cachedItemToBeRemoved = queue.Last();
            queue.RemoveLast();
            return cachedItemToBeRemoved.Key;
        }

        public void OnUpdate(CacheItem<K, V> cacheItem)
        {
            if (cacheItem != queue.First())
            {
                queue.Remove(cacheItem);
                queue.AddFirst(cacheItem);
            }
        }

        public void Remove(CacheItem<K, V> cacheItem)
        {
            queue.Remove(cacheItem);
        }

        public List<K> GetAllKeys()
        {
            List<K> keys = new List<K>();
            foreach(CacheItem<K, V> cacheItem in queue)
            {
                keys.Add(cacheItem.Key);
            }

            return keys;
        }

        public void Clear()
        {
            queue.Clear();
        }
    }
}
