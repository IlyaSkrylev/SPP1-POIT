using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracer.Core.Abstractions;

namespace Tracer.Core
{
    public class Tracer : ITracer
    {
        private Dictionary<int, ThreadInfoResult> threadInfoResults;
        private Dictionary<int, Stack<Stopwatch>> stopwatches;
        private readonly object lockObject = new object();
        public Tracer()
        {
            threadInfoResults = new Dictionary<int, ThreadInfoResult>();
            stopwatches = new Dictionary<int, Stack<Stopwatch>>();
        }

        public void StartTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            lock (lockObject)
            {
                if (!threadInfoResults.ContainsKey(threadId))
                {
                    threadInfoResults[threadId] = new ThreadInfoResult
                    {
                        ThreadId = threadId,
                        Time = 0,
                        Methods = new List<MethodInfoResult>()
                    };
                    stopwatches[threadId] = new Stack<Stopwatch>();
                }
            }

            var methodInfo = new MethodInfoResult
            {
                ClassName = GetCurrentClassName(),
                MethodName = GetCurrentMethodName(),
                Time = 0,
                Methods = new List<MethodInfoResult>()
            };

            PushMethodInfo(threadId, 0, stopwatches[threadId].Count, methodInfo, threadInfoResults[threadId].Methods);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            stopwatches[threadId].Push(stopwatch);
        }

        private void PushMethodInfo(int threadId, int curNesting, int nesting, MethodInfoResult methodInfo, List<MethodInfoResult> threadsInfo)
        {
            if (curNesting != nesting)
            {
                curNesting++;
                PushMethodInfo(threadId, curNesting, nesting, methodInfo, threadsInfo.LastOrDefault().Methods);
            }
            else 
                threadsInfo.Add(methodInfo);
        }

        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (threadInfoResults.ContainsKey(threadId))
            {
                var currentStopwatch = stopwatches[threadId].Pop();
                currentStopwatch.Stop();
                int time = (int)currentStopwatch.Elapsed.TotalMilliseconds;

                var tmp = threadInfoResults[threadId];
                if (stopwatches[threadId].Count == 0)
                    tmp.Time += time;
                threadInfoResults[threadId] = tmp;

                NoteTimeInMethodInfo(threadId, 0, stopwatches[threadId].Count, threadInfoResults[threadId].Methods, time);
            }
        }

        private void NoteTimeInMethodInfo(int threadId, int curNesting, int nesting, List<MethodInfoResult> methods, int time)
        {
            if (curNesting != nesting)
                NoteTimeInMethodInfo(threadId, ++curNesting, nesting, methods.LastOrDefault().Methods, time);
            else 
            {
                var methodInfo = methods.LastOrDefault();
                methodInfo.Time = time;
                methods.RemoveAt(methods.Count - 1);
                methods.Add(methodInfo);
            }
        }

        public TraceResult GetTraceResult()
        {
            var traceResult = new TraceResult();
            traceResult.ThreadInfoResults = new List<ThreadInfoResult>();
            foreach (var res in threadInfoResults)
            {
                traceResult.ThreadInfoResults.Add(res.Value);
            }
            return traceResult;
        }

        private string GetCurrentMethodName()
        {
            var stackTrace = new StackTrace();
            return stackTrace.GetFrame(2)?.GetMethod()?.Name; 
        }

        private string GetCurrentClassName()
        {
            var stackTrace = new StackTrace();
            return stackTrace.GetFrame(2)?.GetMethod()?.DeclaringType?.Name; 
        }
    }
}
