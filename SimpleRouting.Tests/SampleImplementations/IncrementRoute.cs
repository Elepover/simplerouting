using System.Threading.Tasks;
using SimpleRouting.Routing;

namespace SimpleRouting.Tests.SampleImplementations
{
    /// <summary>
    /// Add 1 to the int value but not more than 10. 
    /// </summary>
    public class IncrementRoute : IRoutable<BasicRoutingContext<IntWrapper>>
    {
        public bool IsEligible(BasicRoutingContext<IntWrapper> context) => context.Data.Int < 10;

        public Task ProcessAsync(BasicRoutingContext<IntWrapper> context)
        {
            context.Data.Int++;
            context.Continue = true;
            return Task.CompletedTask;
        }
    }
}
