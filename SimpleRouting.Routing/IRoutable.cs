using System.Threading.Tasks;

namespace SimpleRouting.Routing
{
    public interface IRoutable<TRoutingContext>
        where TRoutingContext : IRoutingContext
    {
        /// <summary>
        /// Checks if the route is eligible for execution.
        /// </summary>
        /// <param name="context">Incoming <typeparamref name="TRoutingContext"/>.</param>
        bool IsEligible(TRoutingContext context);
        /// <summary>
        /// Process this route asynchronously.
        /// </summary>
        Task ProcessAsync(TRoutingContext context);
    }
}
