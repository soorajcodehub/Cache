using System;
namespace NWaySetAssociativeCache
{
    public class CacheItem<K, V>
    {
        K key;
        V itemValue;

        public CacheItem(K key, V itemValue)
        {
            this.key = key;
            this.itemValue = itemValue;
        }

        public V ItemValue { get => itemValue; set => itemValue = value; }
        public K Key { get => key;}
    }
}
