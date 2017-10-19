using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCache
{
    public interface ICacheSetHashCalculator<K, V>
    {
        int GetSetIndex(K key);
    }
}
