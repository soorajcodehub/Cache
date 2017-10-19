using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCache
{
    public class DefaultCacheSetHashCalculator<K, V> : ICacheSetHashCalculator<K, V>
    {
        private IEqualityComparer<K> _comparer;

        int _setCount = 0;

        public DefaultCacheSetHashCalculator(int setCount)
        {
            this._setCount = setCount;
            this._comparer = EqualityComparer<K>.Default;
        }

        public int GetSetIndex(K key)
        {
            int hashCode = Math.Abs(_comparer.GetHashCode(key));
            return hashCode % _setCount;
        }
    }
}
