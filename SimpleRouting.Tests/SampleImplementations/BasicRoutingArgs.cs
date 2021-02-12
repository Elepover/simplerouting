using SimpleRouting.Routing;

namespace SimpleRouting.Tests.SampleImplementations
{
    public class BasicRoutingArgs<T> : IRoutingArgs
    {
        public BasicRoutingArgs(T data)
        {
            Data = data;
        }
        
        public bool Continue { get; set; }
        
        public T Data { get; set; }
    }
}