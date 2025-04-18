
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyApp;

namespace MyApp.Tests
{
    [TestClass]
    public class CacheManagerTests
    {
        private CacheManager _cacheManagerUnderTest;
        
        private string _connectionString;
        private int _cacheSize;
        
        [TestInitialize]
        public void Initialize()
        {  
            _connectionString = "";
            _cacheSize = 0;
            
            _cacheManagerUnderTest = new CacheManager(_connectionString, _cacheSize);
        }

        
        [TestMethod]
        public void AddToCache1Test()
        {
            string key = "";
            string value = "";

            int actual = _cacheManagerUnderTest.AddToCache(key, value);

            int expected = 0;
            Assert.AreEqual(expected, actual);
            Assert.Fail("autogenerated");
        }
    }
}