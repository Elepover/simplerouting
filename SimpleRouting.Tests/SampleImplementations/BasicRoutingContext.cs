using SimpleRouting.Routing;

namespace SimpleRouting.Tests.SampleImplementations
{
    public class BasicRoutingContext<T> : IRoutingContext
    {
        public BasicRoutingContext(T data)
        {
            Data = data;
        }
        
        public bool Continue { get; set; }
        
        public T Data { get; set; }
    }
}