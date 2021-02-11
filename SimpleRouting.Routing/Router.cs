using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRouting.Routing
{
    public class Router<TIncoming, TRoutingArgs> :
        IRoutable<TIncoming, TRoutingArgs>, 
        IEnumerable<IRoutable<TIncoming, TRoutingArgs>>
        where TRoutingArgs : IRoutingArgs<TIncoming>
    {
        #region Constructors and operators
        public Router() {}

        /// <summary>
        /// Get the route at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The route's index.</param>
        public IRoutable<TIncoming, TRoutingArgs> this[int index]
            => RegisteredRoutes.ElementAt(index);
        #endregion

        #region Variables
        /// <summary>
        /// Registered routes in this <see cref="Router{TIncoming, TRoutingArgs}"/>
        /// </summary>
        public LinkedList<IRoutable<TIncoming, TRoutingArgs>> RegisteredRoutes { get; } = new();
        #endregion

        #region Interface implementations
        #region IRoutable implementation

        /// <inheritdoc cref="IRoutable{TIncoming, TRoutingArgs}.IsEligible"/>
        public virtual bool IsEligible(TIncoming incoming)
            => incoming is not null && RegisteredRoutes.Any(route => route.IsEligible(incoming));
            

        /// <inheritdoc cref="IRoutable{TIncoming, TRoutingArgs}.ProcessAsync"/>
        public virtual Task ProcessAsync(TRoutingArgs args)
            => RouteAsync(args);
        #endregion

        #region IEnumerable implementation
        public IEnumerator<IRoutable<TIncoming, TRoutingArgs>> GetEnumerator()
            => new RouterEnumerator<IRoutable<TIncoming, TRoutingArgs>>(RegisteredRoutes);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
        #endregion

        /// <summary>
        /// Add a new route to the last of the routing queue.
        /// </summary>
        /// <param name="newRoute">New route to add.</param>
        public void Add(IRoutable<TIncoming, TRoutingArgs> newRoute)
            => RegisteredRoutes.AddLast(newRoute);
        
        /// <summary>
        /// Route the incoming <typeparamref name="TIncoming"/> and return # of processed routes.
        /// </summary>
        /// <param name="args">Routing arguments.</param>
        /// <returns># of processed routes.</returns>
        public virtual async Task<int> RouteAsync(TRoutingArgs args)
        {
            var routed = 0;
            foreach (var route in RegisteredRoutes)
            {
                if (!route.IsEligible(args.IncomingData)) continue;
                await route.ProcessAsync(args);
                routed++;
                if (!args.Continue) break;
            }

            return routed;
        }
    }
}
