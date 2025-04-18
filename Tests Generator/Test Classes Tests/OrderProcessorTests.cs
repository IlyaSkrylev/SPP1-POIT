
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyApp;

namespace MyApp.Tests
{
    [TestClass]
    public class OrderProcessorTests
    {
        private OrderProcessor _orderProcessorUnderTest;
        private Mock<IService> _serviceMock;
        private int _orderId;
        
        [TestInitialize]
        public void Initialize()
        {  
            _orderId = 0;
            _serviceMock = new Mock<IService>();
            _orderProcessorUnderTest = new OrderProcessor(_orderId, _serviceMock.Object);
        }

        
        [TestMethod]
        public void ProcessOrder1Test()
        {
            _orderProcessorUnderTest.ProcessOrder();

            Assert.Fail("autogenerated");
        }
    }
}