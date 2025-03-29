using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Tracer.Core.Tests
{
    [TestClass]
    public class TracerTests
    {
        [TestMethod]
        public void TestAssertMethodName()
        {
            var tracer = new Tracer();

            var info = tracer.GetTraceResult();
            Assert.IsNotNull(info);
        }
        [TestMethod]
        public void TestAssertMethodName1()
        {
            var tracer = new Tracer();

            var info = tracer.GetTraceResult();
            Assert.IsNotNull(info);
        }
        [TestMethod]
        public void TestAssertMethodName2()
        {
            var tracer = new Tracer();

            var info = tracer.GetTraceResult();
            Assert.IsNotNull(info);
        }
    }
}