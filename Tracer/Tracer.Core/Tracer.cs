using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tracer.Core;

public class Tracer : ITracer
{
    public void StartTrace()
    {
        var threadId = Environment.CurrentManagedThreadId;
        var stackTrace = new StackTrace();
        // GetFrame(0) - this method;
        // GetFrame(1) - the method that called StartTrace
        var method = stackTrace.GetFrame(1)!.GetMethod();
        var methodName = method!.Name;
        var className = method.DeclaringType!.Name;
        
        //TODO: это какая - то хуета
        var info = new MethodInfo(methodName, className, 0);

        _traceResult.GetOrAdd(threadId, new RunningThreadInfo()).RunningMethods.Push(info);
        _stopwatches.GetOrAdd(threadId, new ConcurrentStack<Stopwatch>());
        // Place method info into right place
        var parentMethod = _traceResult[threadId].Methods;
        for (var i = 0; i < _stopwatches[threadId].Count; ++i)
        {
            parentMethod = parentMethod.Last().Methods;
        }
        parentMethod.Add(info);

        // Start time measurement
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        _stopwatches[threadId].Push(stopwatch);
    }

    public void StopTrace()
    {
        var threadId = Environment.CurrentManagedThreadId;
        if (!_stopwatches[threadId].TryPop(out var stopwatch))
            return;
        stopwatch.Stop();
        if (!_traceResult[threadId].RunningMethods.TryPop(out var methodInfo))
            return;
        methodInfo.Milliseconds = stopwatch.ElapsedMilliseconds;
    }

    public TraceResult GetTraceResult()
    {
        return new TraceResult(_traceResult.Select(info => new ThreadInfo(info.Value.Methods, info.Key)).ToList());
    }

    private class RunningThreadInfo
    {
        public List<MethodInfo> Methods { get; set; } = new();
        public ConcurrentStack<MethodInfo> RunningMethods { get; set; } = new();
    }

    // TODO: залупские идентификаторы
    private readonly ConcurrentDictionary<int, RunningThreadInfo> _traceResult = new();
    private readonly ConcurrentDictionary<int, ConcurrentStack<Stopwatch>> _stopwatches = new();
}