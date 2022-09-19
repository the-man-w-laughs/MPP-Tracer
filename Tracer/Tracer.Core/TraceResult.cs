namespace Tracer.Core;
public class TraceResult
{
    public TraceResult(IReadOnlyDictionary<int, ThreadInfo> threads)
    {
        Threads = threads;
    }
    public IReadOnlyDictionary<int, ThreadInfo> Threads { get; }
}