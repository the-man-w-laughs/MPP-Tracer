namespace Tracer.Core
{
    public class MethodInfo
    {
        public MethodInfo(string name, string @class, int ms)
        {
            Name = name;
            Class = @class;
            Milliseconds = ms;
        }

        public string Name { get; }
        public string Class { get; }
        internal long Milliseconds { get; set; }
        //public string Time => $"{Milliseconds}ms"; TODO: move this into serialization modules
        public List<MethodInfo> Methods { get; } = new();
    }
}