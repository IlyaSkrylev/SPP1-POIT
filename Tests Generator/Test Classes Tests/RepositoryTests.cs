
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyApp;

namespace MyApp.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private Repository _repositoryUnderTest;
        
        
        
        [TestInitialize]
        public void Initialize()
        {  
            
            
            _repositoryUnderTest = new Repository();
        }

        
        [TestMethod]
        public void Save1Test()
        {
            string data = "";

            _repositoryUnderTest.Save(data);

            Assert.Fail("autogenerated");
        }
    }
}