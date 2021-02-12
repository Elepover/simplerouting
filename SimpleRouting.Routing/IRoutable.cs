using System.Threading.Tasks;

namespace SimpleRouting.Routing
{
    public interface IRoutable<TRoutingArgs>
        where TRoutingArgs : IRoutingArgs
    {
        /// <summary>
        /// Checks if the route is eligible for execution.
        /// </summary>
        /// <param name="args">Incoming <typeparamref name="TRoutingArgs"/>.</param>
        bool IsEligible(TRoutingArgs args);
        /// <summary>
        /// Process this route asynchronously.
        /// </summary>
        Task ProcessAsync(TRoutingArgs args);
    }
}
