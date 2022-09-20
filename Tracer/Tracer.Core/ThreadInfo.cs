using System.Diagnostics;

namespace Tracer.Core
{
    public class ThreadInfo
    {
        public List<MethodInfo> Methods = new();
        public Stack<MethodInfo> RunninigMethods = new();
    }
}