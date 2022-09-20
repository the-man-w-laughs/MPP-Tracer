using Tracer.Core;

public class A1{
    private ITracer _tracer;

    private B1 b1;

    public A1(ITracer tracer){
        this._tracer = tracer;
        b1 = new(tracer);
    }

    public void a(){
        _tracer.StartTrace();
        Thread.Sleep(100);
        b1.b();
        _tracer.StopTrace();
    }
}

public class B1{
    private ITracer _tracer;

    public B1(ITracer tracer){
        this._tracer = tracer;
    }

    public void b(){
        _tracer.StartTrace();
        Thread.Sleep(100);
        _tracer.StopTrace();
    }
}

public class A2{
    private ITracer _tracer;

    public A2(ITracer tracer){
        this._tracer = tracer;
    }

    public void a(){
        _tracer.StartTrace();
        Thread.Sleep(100);
        _tracer.StopTrace();
    }
}

public class B2{
    private ITracer _tracer;

    public B2(ITracer tracer){
        this._tracer = tracer;
    }

    public void b(){
        _tracer.StartTrace();
        Thread.Sleep(100);
        _tracer.StopTrace();
    }
}