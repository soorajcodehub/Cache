using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCache
{
    public interface ICachePolicyFactory<K, V>
    {
        ICachePolicy<K, V> CreatePolicy();
    }
}
