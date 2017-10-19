using System;
using System.Collections.Generic;

namespace NWaySetAssociativeCache
{
	public interface ICachePolicy<K, V>
	{
		void OnUpdate(CacheItem<K, V> cacheItem);

		void OnAdd(CacheItem<K, V> cacheItem);

        void Remove(CacheItem<K, V> cacheItem);

        void Clear();

        K EvictItem();

        List<K> GetAllKeys();
    }
}
