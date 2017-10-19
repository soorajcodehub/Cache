using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCache
{
    public class LRUPolicyFactory<K, V> : ICachePolicyFactory<K, V>
    {
        public ICachePolicy<K, V> CreatePolicy()
        {
            return new LRUCachePolicy<K, V>();
        }
    }
}
