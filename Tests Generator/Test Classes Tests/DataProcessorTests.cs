
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyApp;

namespace MyApp.Tests
{
    [TestClass]
    public class DataProcessorTests
    {
        private DataProcessor _dataProcessorUnderTest;
        private Mock<ILogger> _loggerMock;
        
        
        [TestInitialize]
        public void Initialize()
        {  
            
            _loggerMock = new Mock<ILogger>();
            _dataProcessorUnderTest = new DataProcessor(_loggerMock.Object);
        }

        
        [TestMethod]
        public void ProcessData1Test()
        {
            int data = 0;

            _dataProcessorUnderTest.ProcessData(data);

            Assert.Fail("autogenerated");
        }
    }
}