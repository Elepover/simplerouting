using System.Threading.Tasks;
using SimpleRouting.Routing;

namespace SimpleRouting.Tests.SampleImplementations
{
    /// <summary>
    /// Add 1 to the int value but not more than 10. 
    /// </summary>
    public class AppendRoute : IRoutable<IntWrapper>
    {
        public bool IsEligible(IntWrapper incoming) => incoming.Int < 10;

        public Task ProcessAsync(IRoutingArgs<IntWrapper> args)
        {
            args.IncomingData.Int++;
            args.Continue = true;
            return Task.CompletedTask;
        }
    }
}
