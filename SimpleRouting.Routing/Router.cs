using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRouting.Routing
{
    public class Router<TIncoming> :
        IRoutable<TIncoming>,
        IEnumerable<IRoutable<TIncoming>>
    {
        #region Constructors and operators
        public Router() {}

        /// <summary>
        /// Get the route at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The route's index.</param>
        public IRoutable<TIncoming> this[int index]
            => RegisteredRoutes.ElementAt(index);
        #endregion

        #region Variables
        /// <summary>
        /// Registered routes in this <see cref="Router{TIncoming}"/>
        /// </summary>
        public LinkedList<IRoutable<TIncoming>> RegisteredRoutes { get; } = new();
        #endregion

        #region Interface implementations
        #region IRoutable implementation
        /// <inheritdoc cref="IRoutable{TIncoming}.IsEligible"/>
        public virtual bool IsEligible(TIncoming incoming) =>
            RegisteredRoutes.Any(route => route.IsEligible(incoming));

        /// <inheritdoc cref="IRoutable{TIncoming}.ProcessAsync"/>
        public virtual Task ProcessAsync(IRoutingArgs<TIncoming> args)
            => RouteAsync(args.IncomingData);
        #endregion

        #region IEnumerable implementation
        public IEnumerator<IRoutable<TIncoming>> GetEnumerator()
            => new RouterEnumerator<IRoutable<TIncoming>>(RegisteredRoutes);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
        #endregion

        /// <summary>
        /// Add a new route to the last of the routing queue.
        /// </summary>
        /// <param name="newRoute">New route to add.</param>
        public void Add(IRoutable<TIncoming> newRoute)
            => RegisteredRoutes.AddLast(newRoute);
        
        /// <summary>
        /// Route the incoming <typeparamref name="TIncoming"/> and return # of processed routes.
        /// </summary>
        /// <param name="incoming">Incoming <typeparamref name="TIncoming"/>.</param>
        /// <returns># of processed routes.</returns>
        public virtual async Task<int> RouteAsync(TIncoming incoming)
        {
            var routed = 0;
            foreach (var route in RegisteredRoutes)
            {
                var args = new BasicRoutingArgs<TIncoming>(incoming);
                if (!route.IsEligible(incoming)) continue;
                await route.ProcessAsync(args);
                routed++;
                if (!args.Continue) break;
            }

            return routed;
        }
    }
}
