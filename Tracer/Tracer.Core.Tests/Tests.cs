namespace Tracer.Core.Tests;

public class Test
{
    [Fact]
    public void SingleThreadOneInner()
    {
        // Arrange 
        Tracer tracer = new();

        // Act 
        A1 a1 = new(tracer);
        a1.a();
        
        int id = Environment.CurrentManagedThreadId;

        // Assert
        var traceResult = tracer.GetTraceResult().Threads;
        Assert.Equal(traceResult.Count,1); 

        Assert.Equal(traceResult[id].Methods.Count,1);
        Assert.Equal(traceResult[id].Methods[0].ClassName,"A1");
        Assert.Equal(traceResult[id].Methods[0].MethodName,"a");
        Assert.InRange<long>(traceResult[id].Methods[0].StopWatch.ElapsedMilliseconds,200,230);

        Assert.Equal(traceResult[id].Methods[0].Methods.Count,1);
        Assert.Equal(traceResult[id].Methods[0].Methods[0].ClassName,"B1");
        Assert.Equal(traceResult[id].Methods[0].Methods[0].MethodName,"b");
        Assert.InRange<long>(traceResult[id].Methods[0].Methods[0].StopWatch.ElapsedMilliseconds,100,120);

    }


    // [Fact]
    // public void MultiThreadWithNoInnerMethods()
    // {
    //     // Arrange
    //     ITracer tracer = new Tracer.Core.Tracer();
    //     A a = new A(tracer);
    //     B b = new B(tracer);

    //     // Act
    //     var t1 = new Thread(() =>
    //     {
    //         a.A2();
    //     });

    //     var t2 = new Thread(() =>
    //     {
    //         b.B2();
    //     });

    //     t1.Start();
    //     t2.Start();
    //     t1.Join();
    //     t2.Join();}   
} 