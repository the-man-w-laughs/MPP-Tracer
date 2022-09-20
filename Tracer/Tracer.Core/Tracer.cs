using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tracer.Core;

public class Tracer : ITracer
{
    public TraceResult GetTraceResult()
    {
        Dictionary<int,ThreadInfo> traceResult = new();
        for (int i = 1; i <= _threads.Count; i++){
            traceResult.Add(i,_threads[i]);
        }
        return new TraceResult(traceResult);
    }

    public void StartTrace()
    {
        int threadId = Environment.CurrentManagedThreadId;

        var threadInfo = _threads.GetOrAdd(threadId,new ThreadInfo());

        StackTrace stackTrace = new();
        var methodInfo = stackTrace.GetFrame(1)!.GetMethod();
        String methodName = methodInfo!.Name;
        String className = methodInfo.DeclaringType!.Name;
            
        Stopwatch stopwatch = new();
        var methodIndo = new MethodInfo(methodName,className,stopwatch);

        if (threadInfo.RunninigMethods.Count != 0){ 
            var parentMethod = threadInfo.RunninigMethods.First();
            parentMethod.Methods.Add(methodIndo);
        }
        else{
            threadInfo.Methods.Add(methodIndo);
        }
        threadInfo.RunninigMethods.Push(methodIndo);

        stopwatch.Start();        
    }

    public void StopTrace()
    {
        int threadId = Environment.CurrentManagedThreadId;
        
        MethodInfo? methodInfo;
        if (!_threads[threadId].RunninigMethods.TryPop(out methodInfo)) return;

        methodInfo.StopWatch.Stop();
    }

    private ConcurrentDictionary<int,ThreadInfo> _threads = new();
}