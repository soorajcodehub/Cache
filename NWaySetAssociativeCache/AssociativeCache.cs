using System;
using System.Collections;
using System.Collections.Generic;

namespace NWaySetAssociativeCache
{
    public class AssociativeCache<K, V>
    {
        int way;
        int totalCapacity;
        int numberOfCacheSets;
        ICacheAlgorithm<K, V> cacheAlgorithm;
        Dictionary<K, CacheSet<K, V>> cacheSetLookUp;


        public AssociativeCache(int way, int totalCapacity, ICacheAlgorithm<K, V> cacheAlgorithm)
        {
            this.way = way;
            this.totalCapacity = totalCapacity;
            this.cacheAlgorithm = cacheAlgorithm;
            this.numberOfCacheSets = this.totalCapacity / this.way;
            this.cacheSetLookUp = new Dictionary<K, CacheSet<K, V>>(this.numberOfCacheSets);
        }

        public void Add(K key, V itemValue)
        {
            if (!cacheSetLookUp.ContainsKey(key))
            {
                cacheSetLookUp.Add(key, new CacheSet<K, V>(this.cacheAlgorithm, totalCapacity / numberOfCacheSets ));
            }
        }

        public bool TryGet(K key, out V itemValue)
        {

            if (cacheSetLookUp.ContainsKey(key))
            {
                itemValue = cacheSetLookUp[key].GetItemFromCacheSet(key);
                return true;
            }

            itemValue = default(V);
            return false;
        }


    }
}
