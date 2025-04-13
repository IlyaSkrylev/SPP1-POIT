namespace Tests
{
    [TestClass]
    public class CallMethods
    {
        [TestMethod]
        public void CreationTracer()
        {
            var tracer = new Tracer.Core.Tracer();
            Assert.IsNotNull(tracer);
        }

        [TestMethod]
        public void MethodAndClassNameTest()
        {
            var tracer = new Tracer.Core.Tracer();
            tracer.StartTrace();
            tracer.StopTrace();

            var info = tracer.GetTraceResult();
            Assert.IsNotNull(info);
            Assert.AreEqual("MethodAndClassNameTest", info.ThreadInfoResults[0].Methods[0].MethodName);
            Assert.AreEqual("CallMethods", info.ThreadInfoResults[0].Methods[0].ClassName);
        }

        [TestMethod]
        public void CallMethodTest()
        {
            var tracer = new Tracer.Core.Tracer();

            testFunc(tracer);

            var info = tracer.GetTraceResult();
            Assert.AreEqual("testFunc", info.ThreadInfoResults[0].Methods[0].MethodName);
            Assert.AreEqual("CallMethods", info.ThreadInfoResults[0].Methods[0].ClassName);
            Assert.AreEqual(1, info.ThreadInfoResults.Count);
            Assert.AreEqual(1, info.ThreadInfoResults[0].Methods.Count);
        }

        [TestMethod]
        public void CallTwoMethodTest()
        {
            var tracer = new Tracer.Core.Tracer();

            testFunc(tracer);
            testFunc(tracer);

            var info = tracer.GetTraceResult();
            Assert.AreEqual(1, info.ThreadInfoResults.Count);
            Assert.AreEqual(2, info.ThreadInfoResults[0].Methods.Count);
        }

        [TestMethod]
        public void CallTwoMethodsTest()
        {
            var tracer = new Tracer.Core.Tracer();

            testFunc2(tracer);

            var info = tracer.GetTraceResult();
            Assert.AreEqual(1, info.ThreadInfoResults.Count);
            Assert.AreEqual(1, info.ThreadInfoResults[0].Methods.Count);
            Assert.AreEqual(1, info.ThreadInfoResults[0].Methods[0].Methods.Count);
            Assert.AreEqual("testFunc2", info.ThreadInfoResults[0].Methods[0].MethodName);
            Assert.AreEqual("testFunc", info.ThreadInfoResults[0].Methods[0].Methods[0].MethodName);
        }

        private void testFunc(Tracer.Core.Tracer tracer)
        {
            tracer.StartTrace();
            tracer.StopTrace();
        }

        private void testFunc2(Tracer.Core.Tracer tracer)
        {
            tracer.StartTrace();
            testFunc(tracer);
            tracer.StopTrace();
        }
    }
}
