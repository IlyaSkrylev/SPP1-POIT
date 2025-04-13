using System.Runtime.CompilerServices;
using Tracer.Core.Abstractions;

namespace Tests
{
    [TestClass]
    public class CallThreads
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
            ITracer tracer = new Tracer.Core.Tracer();

            Thread[] threads = new Thread[3];
            for (int i = 0; i < threads.Length; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() => threadFunc(threadIndex, tracer, testFunc));
                threads[i].Start();
            }

            foreach (Thread thread in threads) thread.Join();

            var info = tracer.GetTraceResult();

            Assert.IsNotNull(info);
            Assert.AreEqual(3, info.ThreadInfoResults.Count);
        }

        [TestMethod]
        public void CallMethodTest()
        {
            ITracer tracer = new Tracer.Core.Tracer();

            Thread[] threads = new Thread[3];
            for (int i = 0; i < threads.Length; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() => threadFunc(threadIndex, tracer, testFunc));
                threads[i].Start();
            }

            foreach (Thread thread in threads) thread.Join();

            var info = tracer.GetTraceResult();

            Assert.IsNotNull(info);
            Assert.AreEqual(3, info.ThreadInfoResults.Count);
            Assert.AreEqual(1, info.ThreadInfoResults[0].Methods.Count);
        }

        [TestMethod]
        public void CallTwoMethodTest()
        {
            ITracer tracer = new Tracer.Core.Tracer();

            Thread[] threads = new Thread[3];
            for (int i = 0; i < threads.Length; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() => threadFunc(threadIndex, tracer, testFunc2));
                threads[i].Start();
            }

            foreach (Thread thread in threads) thread.Join();

            var info = tracer.GetTraceResult();

            Assert.IsNotNull(info);
            Assert.AreEqual(3, info.ThreadInfoResults.Count);
            Assert.AreEqual(1, info.ThreadInfoResults[0].Methods.Count);
            Assert.AreEqual(1, info.ThreadInfoResults[2].Methods[0].Methods.Count);
        }


        private void threadFunc(int threadIndex, ITracer _tracer, Action<ITracer> func)
        {
            func(_tracer);
        }

        private void testFunc(ITracer tracer)
        {
            tracer.StartTrace();
            tracer.StopTrace();
        }

        private void testFunc2(ITracer tracer)
        {
            tracer.StartTrace();
            testFunc(tracer);
            tracer.StopTrace();
        }
    }
}
