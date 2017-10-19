using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Appender;
using log4net.Core;

namespace NWaySetAssociativeCache
{
    public class MRUCachePolicy<K, V> : ICachePolicy<K, V>
    {
        LinkedList<CacheItem<K, V>> queue;

        public MRUCachePolicy(AppenderSkeleton appender = null)
        {
            this.queue = new LinkedList<CacheItem<K, V>>();
        }

        public void OnAdd(CacheItem<K, V> cacheItem)
        {
            queue.AddFirst(cacheItem);
        }

        public K EvictItem()
        {
            CacheItem<K, V> cachedItemToBeRemoved = queue.First();
            queue.RemoveFirst();
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

        public List<K> GetAllKeys()
        {
            List<K> keys = new List<K>();
            foreach (CacheItem<K, V> cacheItem in queue)
            {
                keys.Add(cacheItem.Key);
            }

            return keys;
        }

        public void Remove(CacheItem<K, V> cacheItem)
        {
            queue.Remove(cacheItem);
        }

        public void Clear()
        {
            queue.Clear();
        }
    }
}
