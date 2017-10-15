using System;
using System.Collections;
using System.Collections.Generic;

namespace NWaySetAssociativeCache
{
    public class CacheSet<K, V>
    {
        Dictionary<K, CacheItem<K, V>> cacheItemLookUp;
        int setCapacity;
        ICacheAlgorithm<K, V> cacheAlgorithm;

        public CacheSet(ICacheAlgorithm<K, V> cacheAlgorithm, int setCapacity)
        {
            this.setCapacity = setCapacity;
            cacheItemLookUp = new Dictionary<K, CacheItem<K, V>>(setCapacity);
            this.cacheAlgorithm  = cacheAlgorithm;
        }

        public void AddItemToCacheSet(K key, V itemValue)
        {
            this.cacheAlgorithm.PutItem(key, itemValue);
        }

        public V GetItemFromCacheSet(K key)
        {
           return this.cacheAlgorithm.GetItem(key);
        }
    }
}
