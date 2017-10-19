using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCache
{
    public class MRUPolicyFactory<K, V> : ICachePolicyFactory<K, V>
    {
        public ICachePolicy<K, V> CreatePolicy()
        {
            return new MRUCachePolicy<K, V>();
        }
    }
}
