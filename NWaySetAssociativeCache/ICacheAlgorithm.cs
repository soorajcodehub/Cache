using System;
namespace NWaySetAssociativeCache
{
	public interface ICacheAlgorithm<K, V>
	{
		V GetItem(K Key);

		void PutItem(K key, V value);
	}
}
