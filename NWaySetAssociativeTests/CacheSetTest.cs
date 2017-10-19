using System;
using NWaySetAssociativeCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NWaySetAssociativeTests
{
    [TestClass]
    public class CachePolicyTests
    {
        [TestMethod]
        public void LRUCache_LeastRecentlyItemShouldBeAtTheEnd()
        {
            var cacheSet = new CacheSet<string, string>(new LRUCachePolicy<string, string>(), 4);
            cacheSet.AddItemToCacheSet("A", "dataA");
            cacheSet.AddItemToCacheSet("B", "dataB");
            cacheSet.AddItemToCacheSet("C", "dataC");
            cacheSet.AddItemToCacheSet("D", "dataD");
            List<string> expected = new List<string>() { "D", "C", "B", "A" };
            List<string> actual = cacheSet.GetAllKeys();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[3], actual[3]);
        }

        [TestMethod]
        public void LRUCache_LeastRecentlyItemShouldBeAtTheEndWithEviction()
        {
            var cacheSet = new CacheSet<string, string>(new LRUCachePolicy<string, string>(), 4);

            cacheSet.AddItemToCacheSet("W", "dataW");
            cacheSet.AddItemToCacheSet("X", "dataX");
            cacheSet.AddItemToCacheSet("W", "dataWW");
            cacheSet.AddItemToCacheSet("Y", "dataY");
            cacheSet.AddItemToCacheSet("Z", "dataZ");
            cacheSet.AddItemToCacheSet("W", "dataWWW");
            cacheSet.AddItemToCacheSet("T", "dataT");

            var expected = new List<string>() { "T", "W", "Z", "Y" };
            var actual = cacheSet.GetAllKeys();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[3], actual[3]);

            cacheSet.AddItemToCacheSet("Y", "dataYY");
            cacheSet.AddItemToCacheSet("Z", "dataZZ");
            cacheSet.AddItemToCacheSet("W", "dataWWW");
            cacheSet.AddItemToCacheSet("Y", "dataYYY");
            cacheSet.AddItemToCacheSet("X", "dataXX");

            expected = new List<string>() { "X", "Y", "W", "Z" };
            actual = cacheSet.GetAllKeys();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[3], actual[3]);

            cacheSet.AddItemToCacheSet("X", "dataXX");
            cacheSet.AddItemToCacheSet("Y", "dataYYYY");
            cacheSet.AddItemToCacheSet("X", "dataXXX");
            cacheSet.AddItemToCacheSet("Y", "dataYY");
            expected = new List<string>() { "Y", "X", "W", "Z" };
            actual = cacheSet.GetAllKeys();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[3], actual[3]);
        }

        [TestMethod]
        public void MRUCache_MostRecentlyItemShouldBeAtTheEnd()
        {
            var cacheSet = new CacheSet<string, string>(new MRUCachePolicy<string, string>(), 4);
            cacheSet.AddItemToCacheSet("A", "dataA");
            cacheSet.AddItemToCacheSet("B", "dataB");
            cacheSet.AddItemToCacheSet("C", "dataC");
            cacheSet.AddItemToCacheSet("D", "dataD");
            List<string> expected = new List<string>() { "D", "C", "B", "A" };
            List<string> actual = cacheSet.GetAllKeys();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[3], actual[3]);
        }

        [TestMethod]
        public void MRUCache_MostRecentlyItemShouldBeAtTheEndWithEviction()
        {
            var cacheSet = new CacheSet<string, string>(new MRUCachePolicy<string, string>(), 4);

            cacheSet.AddItemToCacheSet("W", "dataW");
            cacheSet.AddItemToCacheSet("X", "dataX");
            cacheSet.AddItemToCacheSet("W", "dataWW");
            cacheSet.AddItemToCacheSet("Y", "dataY");
            cacheSet.AddItemToCacheSet("Z", "dataZ");
            cacheSet.AddItemToCacheSet("W", "dataWWW");
            cacheSet.AddItemToCacheSet("T", "dataT");

            var expected = new List<string>() { "T", "Z", "Y", "X"  };
            var actual = cacheSet.GetAllKeys();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[3], actual[3]);

            cacheSet.AddItemToCacheSet("Y", "dataYY");
            cacheSet.AddItemToCacheSet("Z", "dataZZ");
            cacheSet.AddItemToCacheSet("W", "dataWWW");
            cacheSet.AddItemToCacheSet("Y", "dataYYY");
            cacheSet.AddItemToCacheSet("X", "dataXX");

            expected = new List<string>() { "X", "Y", "W", "T" };
            actual = cacheSet.GetAllKeys();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[3], actual[3]);

            cacheSet.AddItemToCacheSet("X", "dataXX");
            cacheSet.AddItemToCacheSet("Y", "dataYYYY");
            cacheSet.AddItemToCacheSet("X", "dataXXX");
            cacheSet.AddItemToCacheSet("Y", "dataYY");
            expected = new List<string>() { "Y", "X", "W", "T" };
            actual = cacheSet.GetAllKeys();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[3], actual[3]);
        }
    }
}
