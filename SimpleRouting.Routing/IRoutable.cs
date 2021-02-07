using System.Threading.Tasks;

namespace SimpleRouting.Routing
{
    public interface IRoutable<TIncoming>
    {
        /// <summary>
        /// Checks if the route is eligible for execution.
        /// </summary>
        /// <param name="incoming">Incoming <typeparamref name="TIncoming"/>.</param>
        bool IsEligible(TIncoming incoming);
        /// <summary>
        /// Process this route asynchronously.
        /// </summary>
        Task ProcessAsync(IRoutingArgs<TIncoming> args);
    }
}