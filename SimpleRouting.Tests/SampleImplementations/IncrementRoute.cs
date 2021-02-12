using System.Threading.Tasks;
using SimpleRouting.Routing;

namespace SimpleRouting.Tests.SampleImplementations
{
    /// <summary>
    /// Add 1 to the int value but not more than 10. 
    /// </summary>
    public class IncrementRoute : IRoutable<BasicRoutingArgs<IntWrapper>>
    {
        public bool IsEligible(BasicRoutingArgs<IntWrapper> args) => args.Data.Int < 10;

        public Task ProcessAsync(BasicRoutingArgs<IntWrapper> args)
        {
            args.Data.Int++;
            args.Continue = true;
            return Task.CompletedTask;
        }
    }
}
