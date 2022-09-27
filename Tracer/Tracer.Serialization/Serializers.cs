using System.Reflection;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization;
public static class Serializers
{
    public static List<ITraceResultSerializer> GetSerializers(string path)
    {

        List<ITraceResultSerializer> serializers = new();
        try
        {
            DirectoryInfo pluginDirectory = new DirectoryInfo(path);
            var pluginFiles = Directory.GetFiles(path, "*.dll");

            foreach (var file in pluginFiles)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);

                    var types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        // TODO: isAssignablefrom - для проверки совметримости типа с интерфейнсом
                        // TODO: интересные try catch
                        try
                        {
                            if (type.IsAssignableFrom(typeof(ITraceResultSerializer)))
                            {
                                ITraceResultSerializer? serializer = assembly.CreateInstance(type.FullName!) as ITraceResultSerializer;
                                serializers.Add(serializer!);
                            }
                        }
                        catch { }
                    }
                }
                catch
                {

                }
            }
        }
        catch
        {

        }
        return serializers;


    }

    public static void SerializeIntoFiles(Core.TraceResult traceResult, String serializersFolderPath, String resultFolder)
    {
        if (!Directory.Exists(resultFolder))
        {
            Directory.CreateDirectory(resultFolder);
        }
        List<ITraceResultSerializer> serializers = Serializers.GetSerializers(serializersFolderPath);
        foreach (var serializer in serializers)
        {
            using (var fileStream = new FileStream(Path.Combine(resultFolder, $"result.{serializer.Format}"), FileMode.Create))
            {
                serializer.Serialize(traceResult, fileStream);
            }
        }
    }
}
