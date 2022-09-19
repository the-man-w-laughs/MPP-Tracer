using Tracer.Core;

namespace Tracer.Serialization.Abstractions;
public interface ITraceResultSerializer
{
    // Опционально: возвращает формат, используемый сериализатором (xml/json/yaml).
    // Может быть удобно для выбора имени файлов (см. ниже).
    public string Format { get; }
    public void Serialize(TraceResult traceResult, Stream to);
}