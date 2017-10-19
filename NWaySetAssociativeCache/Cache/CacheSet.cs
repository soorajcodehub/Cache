using System.Collections.Generic;
using log4net.Appender;
using log4net.Core;

namespace NWaySetAssociativeCache
{
    public class CacheSet<K, V>
    {
        Dictionary<K, CacheItem<K, V>> cacheItemLookUp;
        int setCapacity;
        ICachePolicy<K, V> cachePolicy;
        private readonly object cacheSetReadWriteGuard = new object();
        AppenderSkeleton appender;

        public int SetCapacity { get => setCapacity; }

        public ICachePolicy<K, V> CachePolicy { get => cachePolicy; }

        public CacheSet(ICachePolicy<K, V> cachePolicy, int setCapacity, AppenderSkeleton appender = null)
        {
            this.setCapacity = setCapacity;
            cacheItemLookUp = new Dictionary<K, CacheItem<K, V>>(setCapacity);
            this.cachePolicy = cachePolicy;
            this.appender = appender ?? new ConsoleAppender();
        }

        public void AddItemToCacheSet(K key, V itemValue)
        {
            lock (this.cacheSetReadWriteGuard)
            {
                if (cacheItemLookUp.TryGetValue(key, out CacheItem<K, V> existingCacheItem))
                {
                    existingCacheItem.ItemValue = itemValue;
                    LoggingUtility.LogData(appender, $"Updated CacheIem with {key}", Level.Trace);
                    CachePolicy.OnUpdate(existingCacheItem);
                }
                else
                {
                    this._evictIfNeeded();
                    CacheItem<K, V> cacheItem = new CacheItem<K, V>(key, itemValue);
                    cacheItemLookUp.Add(key, cacheItem);
                    CachePolicy.OnAdd(cacheItem);
                    LoggingUtility.LogData(appender, $"Added CacheIem with {key}", Level.Trace);
                }
            }
        }

        public V GetItemFromCacheSet(K key)
        {
            lock (this.cacheSetReadWriteGuard)
            {
                if (this.cacheItemLookUp.TryGetValue(key, out CacheItem<K, V> cacheItem))
                {
                    CachePolicy.OnUpdate(cacheItem);
                    LoggingUtility.LogData(appender, $"Cache hit on CacheIem with {key}", Level.Trace);
                    return cacheItem.ItemValue;
                }
                else
                {
                    return default(V);
                }
            }
        }

        public void RemoveItemFromCacheSet(K key)
        {
            lock (this.cacheSetReadWriteGuard)
            {
                if (this.cacheItemLookUp.TryGetValue(key, out CacheItem<K, V> cacheItem))
                {
                    CachePolicy.Remove(cacheItem);
                    cacheItemLookUp.Remove(key);
                    LoggingUtility.LogData(appender, $"Cache removal on CacheIem with {key}", Level.Trace);
                }
                else
                {
                    LoggingUtility.LogData(appender, $"Cache miss on CacheIem with {key}", Level.Error);
                }
            }
        }

        public void Clear()
        {
            lock (this.cacheSetReadWriteGuard)
            {
                CachePolicy.Clear();
                this.cacheItemLookUp.Clear();
            }
        }

        public List<K> GetAllKeys()
        {
            lock (this.cacheSetReadWriteGuard)
            {
                return this.CachePolicy.GetAllKeys();
            }
        }

        private void _evictIfNeeded()
        {
            if (cacheItemLookUp.Count >= SetCapacity)
            {
                K evictedItemKey = this.CachePolicy.EvictItem();
                this.cacheItemLookUp.Remove(evictedItemKey);
                LoggingUtility.LogData(appender, $"Evicted CacheIem with {evictedItemKey}", Level.Trace);
            }
        }
    }
}
