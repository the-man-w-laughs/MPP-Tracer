using Tracer.Core;
using Tracer.Serialization;
using Tracer.Serialization.Abstractions;

internal class Program
{
    private static void Main(string[] args)
    {
        Tracer.Core.Tracer tracer = new();
        Foo foo = new(tracer);
        foo.MyMethod();
        var traceResult = tracer.GetTraceResult();
        List<ITraceResultSerializer> serializers = SerializersLoader.GetSerializers("Serializers");
        foreach (var serializer in serializers){
            using (var fileStream = new FileStream($"result.{serializer.Format}", FileMode.Create)){
                    serializer.Serialize(traceResult, fileStream);
                }
        }
        }
}

public class Foo
{
    private Bar _bar;
    private ITracer _tracer;

    internal Foo(ITracer tracer)
    {
        _tracer = tracer;
        _bar = new Bar(_tracer);
    }
    
    public void MyMethod()
    {
        _tracer.StartTrace();
        _bar.InnerMethod();        
        _tracer.StopTrace();
    }
}

public class Bar
{
    private ITracer _tracer;

    internal Bar(ITracer tracer)
    {
        _tracer = tracer;
    }
    
    public void InnerMethod()
    {
        _tracer.StartTrace();
        _tracer.StopTrace();
    }
}