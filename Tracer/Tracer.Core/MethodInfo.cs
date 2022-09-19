using System.Diagnostics;

namespace Tracer.Core
{
    public class MethodInfo
    {
        public MethodInfo(string methodName, string className, Stopwatch stopwatch)
        {
            MethodName = methodName;
            ClassName = className;
            StopWatch = stopwatch;
        }

        public string MethodName { get; }
        public string ClassName { get; }
        public List<MethodInfo> Methods { get; } = new();
        public Stopwatch StopWatch;
    }
}