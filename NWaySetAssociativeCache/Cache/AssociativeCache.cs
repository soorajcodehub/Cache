using System;
using System.Collections.Generic;
using log4net.Appender;
using log4net.Core;

namespace NWaySetAssociativeCache
{
    public class AssociativeCache<K, V>
    {
        ICachePolicyFactory<K, V> cachePolicyFactory;
        Dictionary<int, CacheSet<K, V>> cacheSetLookUp;
        ICacheSetHashCalculator<K, V> hashSetCalculator;
        AppenderSkeleton appender;

        public AssociativeCache(
            int way,
            int totalCacheCapacity,
            ICachePolicyFactory<K, V> cachePolicyFactory = null,
            ICacheSetHashCalculator<K, V> hashSetCacluator = null,
            AppenderSkeleton appender = null)
        {
            this.appender = appender ?? new ConsoleAppender();

            // Implement as a one way cache
            if (way <= 0)
            {
                LoggingUtility.LogData(this.appender, "Number of cache sets cannot be 0 or negative", Level.Fatal);
                throw new ArgumentOutOfRangeException("Number of cache sets cannot be 0 or negative", "way");
            }

            if (totalCacheCapacity <= 0)
            {
                LoggingUtility.LogData(this.appender, "Number of cache sets cannot be 0 or negative", Level.Fatal);
                throw new ArgumentOutOfRangeException("Number of cache sets cannot be 0 or negative", "totalCacheCapacity");
            }

            this.cachePolicyFactory = cachePolicyFactory;
            this.cacheSetLookUp = new Dictionary<int, CacheSet<K, V>>();
            this.cachePolicyFactory = cachePolicyFactory ?? new LRUPolicyFactory<K, V>();
            this.hashSetCalculator = hashSetCalculator ?? new DefaultCacheSetHashCalculator<K, V>(way);

            for (int i = 0; i < way; i++)
            {
                this.cacheSetLookUp.Add(i, new CacheSet<K, V>(this.cachePolicyFactory.CreatePolicy(), totalCacheCapacity / way, this.appender));
            }
        }

        public void Add(K key, V itemValue)
        {
            try
            {
                int setIndex = this.hashSetCalculator.GetSetIndex(key);
                CacheSet<K, V> cacheSet = cacheSetLookUp[setIndex];
                cacheSet.AddItemToCacheSet(key, itemValue);
                LoggingUtility.LogData(appender, $"Adding CacheIem in {setIndex} cache set", Level.Trace);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogData(appender, ex.Message, Level.Fatal);
                throw;
            }
        }

        public void Remove(K key)
        {
            try
            {
                int setIndex = this.hashSetCalculator.GetSetIndex(key);
                CacheSet<K, V> cacheSet = cacheSetLookUp[setIndex];
                cacheSet.RemoveItemFromCacheSet(key);
                LoggingUtility.LogData(appender, $"Adding CacheIem in {setIndex} cache set", Level.Trace);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogData(appender, ex.Message, Level.Fatal);
                throw;
            }
        }

        public void Clear()
        {
            try
            {
                foreach(CacheSet<K, V> cacheSet in cacheSetLookUp.Values)
                {
                    cacheSet.Clear();
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogData(appender, ex.Message, Level.Fatal);
                throw;
            }
        }

        public V Get(K key)
        {
            try
            {
                int setIndex = this.hashSetCalculator.GetSetIndex(key);

                if (cacheSetLookUp[setIndex] != null)
                {
                    LoggingUtility.LogData(appender, $"Possible cache hit in set {setIndex}", Level.Trace);
                    return cacheSetLookUp[setIndex].GetItemFromCacheSet(key);
                }
                else
                {
                    LoggingUtility.LogData(appender, $"Cache miss in set {setIndex} for key {key}", Level.Trace);
                    return default(V);
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogData(appender, ex.Message, Level.Fatal);
                throw;
            }
        }

        public List<CacheSet<K, V>> GetCacheSets()
        {
            List<CacheSet<K, V>> cacheSets = new List<CacheSet<K, V>>();

            foreach (CacheSet<K, V> cacheSet in this.cacheSetLookUp.Values)
            {
                cacheSets.Add(cacheSet);
            }

            return cacheSets;
        }
          

    }
}
