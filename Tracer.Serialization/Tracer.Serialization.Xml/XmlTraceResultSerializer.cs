namespace Tracer.Serialization.Json;

using System.Xml.Serialization;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

public class XmlTraceResultSerializer : ITraceResultSerializer
{
    public string Format{ 
        get {
            return "Xml";
        }
    }

    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class thread{

        public thread(){
            
        }
        public thread(int id,long time,List<method> methods){
            this.id = id;
            this.time = time;
            this.methods = methods;
        }

       // [System.Xml.Serialization.XmlAttributeAttribute()]
        public int id{get; set;}
       // [System.Xml.Serialization.XmlAttributeAttribute()]
        public long time{get; set;}

        public List<method> methods;        
    
    }

    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class method{
        public method(){

        }
        public method(MethodInfo method){

            this.name = method.MethodName;
            this.Class = method.ClassName;
            this.time = $"{method.StopWatch.ElapsedMilliseconds}ms";
            if (method.Methods != null){
                this.methods = new List<method>();
                foreach (var currMethod in method.Methods){
                    this.methods.Add(new method(currMethod));
                }
            }
            else {
                this.methods = null;
            }
        }

        //[System.Xml.Serialization.XmlAttributeAttribute()]
        public String name{get; set;}
        //[System.Xml.Serialization.XmlAttributeAttribute()]
        [XmlElement(ElementName = "class")]
        public String Class{get; set;}

        //[System.Xml.Serialization.XmlAttributeAttribute()]
        public String time{get; set;}

        public List<method>? methods;
    }

    private List<thread> _ThreadResultToList(TraceResult traceResult){
        List<thread> resultList= new();
        List<method> methods;
        long time;
        for (int i = 1; i <= traceResult.Threads.Count; i++){  
            methods = new();
            time = 0;
            foreach (var method in traceResult.Threads[i].Methods){
                methods.Add(new method(method));
                time += method.StopWatch.ElapsedMilliseconds;
            }
            resultList.Add(new thread(i,time,methods));
        }
        return resultList;
    }  
    public void Serialize(TraceResult traceResult, Stream to)
    {
        List<thread> serializableList = _ThreadResultToList(traceResult);
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<thread>));
        xmlSerializer.Serialize(to, serializableList);
    }
}
