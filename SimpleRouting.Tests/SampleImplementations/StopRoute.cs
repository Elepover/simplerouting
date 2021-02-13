using System.Threading.Tasks;
using SimpleRouting.Routing;
using SimpleRouting.Routing.Extensions;

namespace SimpleRouting.Tests.SampleImplementations
{
    public class StopRoute : IRoutable<BasicRoutingContext<IntWrapper>>
    {
        public bool IsEligible(BasicRoutingContext<IntWrapper> context) => true;

        public Task ProcessAsync(BasicRoutingContext<IntWrapper> context)
        {
            context.Stop();
            return Task.CompletedTask;
        }
    }
}