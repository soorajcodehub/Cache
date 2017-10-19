using System;
using NWaySetAssociativeCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NWaySetAssociativeTests
{
    [TestClass]
    public class NSetAssociativeCacheTests
    {
        [TestMethod]
        public void AssociativeCache_ShouldInitialize_LRUCachePolicy()
        {
            AssociativeCache<string, string> cache = new AssociativeCache<string, string>(2, 10);
            Assert.AreEqual(5, cache.GetCacheSets()[0].SetCapacity);
            Assert.IsTrue(cache.GetCacheSets().TrueForAll(s => s.CachePolicy.GetType() == typeof(LRUCachePolicy<string, string>)));
        }

        [TestMethod]
        public void AssociativeCache_ShouldInitialize_SpecificCachePolicy()
        {
            AssociativeCache<string, string> cache = new AssociativeCache<string, string>(2, 10, new MRUPolicyFactory<string, string>());
            Assert.AreEqual(5, cache.GetCacheSets()[0].SetCapacity);
            Assert.IsTrue(cache.GetCacheSets().TrueForAll(s => s.CachePolicy.GetType() == typeof(MRUCachePolicy<string, string>)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssociativeCache_ShouldThrowExceptionIfNumberOfSetsAreZero()
        {
            AssociativeCache<string, string> cache = new AssociativeCache<string, string>(0, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssociativeCache_ShouldThrowExceptionIfNumberOfSetsIsBelowZero()
        {
            AssociativeCache<string, string> cache = new AssociativeCache<string, string>(-9, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssociativeCache_ShouldThrowExceptionIfSetCapacityIsBelowZero()
        {
            AssociativeCache<string, string> cache = new AssociativeCache<string, string>(3, -1);
        }

        [TestMethod]
        public void AssociativeCache_AddAndGetTest()
        {
            AssociativeCache<string, string> cache = new AssociativeCache<string, string>(3, 24);

            cache.Add("A", "dataA");
            cache.Add("B", "dataB");
            cache.Add("C", "dataC");
            cache.Add("D", "dataD");
            cache.Add("E", "dataE");
            cache.Add("F", "dataF");

            Assert.AreEqual("dataA", cache.Get("A"));
            Assert.AreEqual("dataB", cache.Get("B"));
            Assert.AreEqual("dataC", cache.Get("C"));
            Assert.AreEqual("dataD", cache.Get("D"));
            Assert.AreEqual("dataE", cache.Get("E"));
            Assert.AreEqual("dataF", cache.Get("F"));
        }

        [TestMethod]
        public void AssociativeCache_Should_Evict_By_LRUPolicy()
        {
            AssociativeCache<int, string> cache = new AssociativeCache<int, string>(3, 6, new LRUPolicyFactory<int, string>());

            cache.Add(1, "dataA");
            cache.Add(2, "dataB");
            cache.Add(3, "dataC");
            cache.Add(4, "dataD");
            cache.Add(5, "dataE");
            cache.Add(6, "dataF");
            cache.Add(7, "dataAA");

            Assert.AreEqual(cache.Get(1), null);
        }

        [TestMethod]
        public void AssociativeCache_Should_Evict_By_MRUPolicy()
        {
            AssociativeCache<int, string> cache = new AssociativeCache<int, string>(3, 6, new MRUPolicyFactory<int, string>());

            cache.Add(1, "dataA");
            cache.Add(2, "dataB");
            cache.Add(3, "dataC");
            cache.Add(4, "dataD");
            cache.Add(5, "dataE");
            cache.Add(6, "dataF");
            cache.Add(7, "dataAA");

            Assert.AreEqual(cache.Get(4), null);
        }

        [TestMethod]
        public void AssociativeCache_Remove_Ok()
        {
            AssociativeCache<int, string> cache = new AssociativeCache<int, string>(3, 6);
            cache.Add(1, "dataA");
            cache.Remove(1);
            Assert.IsNull(cache.Get(1));
        }

        [TestMethod]
        public void AssociativeCache_ComplexKeyIsOk()
        {
            AssociativeCache<MockKey, MockValue> cache = new AssociativeCache<MockKey, MockValue>(3, 6);
            var key = new MockKey { prop = "prop", prop1 = "prop1", prop2 = "prop2" };
            var value1 = new MockValue { prop = "value", prop1 = "value1", prop2 = "value2" };
            cache.Add(key, value1);
            Assert.AreEqual(value1, cache.Get(key));

            var key2 = new MockKey { prop = "prop", prop1 = "prop11" };
            var value2 = new MockValue { prop = "value", prop1 = "value11" };
            cache.Add(key2, value2);
            Assert.AreNotEqual(value2, cache.Get(key));
        }

        [TestMethod]
        public void AssociativeCache_CLear()
        {
            AssociativeCache<string, string> cache = new AssociativeCache<string, string>(3, 6);
            cache.Add("A", "dataA");
            cache.Add("B", "dataA");
            cache.Add("C", "dataA");
            cache.Add("D", "dataA");
            cache.Add("E", "dataA");

            cache.Clear();
            foreach(CacheSet<string, string> cacheSet in cache.GetCacheSets())
            {
                Assert.AreEqual(cacheSet.GetAllKeys().Count, 0);
            }
        }
    }
}
