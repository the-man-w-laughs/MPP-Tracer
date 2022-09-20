namespace Tracer.Serialization.Json;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

public class JsonTraceResultSerializer : ITraceResultSerializer
{
    public string Format{ 
        get {
            return "Json";
        }
    }

    public class _ThreadInfo{

        public _ThreadInfo(int id,long time,List<_MethodInfo> methods){
            this.id = id;
            this.time = $"{time}ms";
            this.methods = methods;
        }

        public int id{get; set;}

        public String time{get; set;}

        public List<_MethodInfo> methods{get; set;}      
    
    }

    public class _MethodInfo{

        public _MethodInfo(MethodInfo method){

            this.name = method.MethodName;
            this.Class = method.ClassName;
            this.time = $"{method.StopWatch.ElapsedMilliseconds}ms";
            if (method.Methods != null){
                this.methods = new List<_MethodInfo>();
                foreach (var currMethod in method.Methods){
                    this.methods.Add(new _MethodInfo(currMethod));
                }
            }
            else {
                this.methods = null;
            }
        }
        public String name{get; set;}

        [JsonPropertyName("class")]
        public String Class{get; set;}

        public String time{get; set;}

        public List<_MethodInfo>? methods{get; set;}
    }

    private List<_ThreadInfo> _ThreadResultToList(TraceResult traceResult){
        List<_ThreadInfo> resultList= new();
        List<_MethodInfo> methods;
        long time;
        foreach (var thread in traceResult.Threads){
            methods = new();
            time = 0;
            foreach (var method in thread.Value.Methods){
                methods.Add(new _MethodInfo(method));
                time += method.StopWatch.ElapsedMilliseconds;
            }
            resultList.Add(new _ThreadInfo(thread.Key,time,methods));
        }
        return resultList;
    }  
    public void Serialize(TraceResult traceResult, Stream to)
    {
        List<_ThreadInfo> serializableList = _ThreadResultToList(traceResult);
        var res = JsonSerializer.Serialize(serializableList);
        to.Write(Encoding.Default.GetBytes(res));
    }
}
