using System.Reflection;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization;
public static class SerializersLoader
{
    public static List<ITraceResultSerializer> GetSerializers(string path)
        {
            List<ITraceResultSerializer> serializers = new();
            DirectoryInfo pluginDirectory = new DirectoryInfo(path);
            var pluginFiles = Directory.GetFiles(path, "*.dll");

            foreach (var file in pluginFiles)
            {
                Assembly assembly = Assembly.LoadFrom(file);

                var types = assembly.GetTypes();
                foreach (var type in types)
                {           
                    var interfaces = type.GetInterfaces(); 
                    foreach (var inter in interfaces)
                    if (inter.FullName == typeof(ITraceResultSerializer).FullName){
                        ITraceResultSerializer? serializer = assembly.CreateInstance(type.FullName!) as ITraceResultSerializer;
                        if (serializer != null)
                        {
                            serializers.Add(serializer);
                        }
                    }                    
                }
            }
            return serializers;
        }
}
