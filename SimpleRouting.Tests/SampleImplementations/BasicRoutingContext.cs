using SimpleRouting.Routing;

namespace SimpleRouting.Tests.SampleImplementations
{
    public class BasicRoutingContext<T> : IRoutingContext
    {
        public BasicRoutingContext(T data)
        {
            Data = data;
        }

        public RouteTarget Target { get; set; } = RouteTarget.Continue;
        
        public T Data { get; set; }
    }
}