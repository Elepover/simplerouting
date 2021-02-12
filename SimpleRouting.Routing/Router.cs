using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRouting.Routing
{
    public class Router<TRoutingArgs> :
        IRoutable<TRoutingArgs>, 
        IEnumerable<IRoutable<TRoutingArgs>>
        where TRoutingArgs : IRoutingArgs
    {
        #region Constructors and operators
        /// <summary>
        /// Get the route at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The route's index.</param>
        public IRoutable<TRoutingArgs> this[int index]
            => RegisteredRoutes.ElementAt(index);
        #endregion

        #region Variables
        /// <summary>
        /// Registered routes in this <see cref="Router{TRoutingArgs}"/>
        /// </summary>
        public LinkedList<IRoutable<TRoutingArgs>> RegisteredRoutes { get; } = new();
        #endregion

        #region Interface implementations
        #region IRoutable implementation

        /// <inheritdoc cref="IRoutable{TRoutingArgs}.IsEligible"/>
        public virtual bool IsEligible(TRoutingArgs args)
            => RegisteredRoutes.Any(route => route.IsEligible(args));
            

        /// <inheritdoc cref="IRoutable{TRoutingArgs}.ProcessAsync"/>
        public virtual Task ProcessAsync(TRoutingArgs args)
            => RouteAsync(args);
        #endregion

        #region IEnumerable implementation
        public IEnumerator<IRoutable<TRoutingArgs>> GetEnumerator()
            => new RouterEnumerator<IRoutable<TRoutingArgs>>(RegisteredRoutes);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
        #endregion

        /// <summary>
        /// Add a new route to the last of the routing queue.
        /// </summary>
        /// <param name="newRoute">New route to add.</param>
        public void Add(IRoutable<TRoutingArgs> newRoute)
            => RegisteredRoutes.AddLast(newRoute);
        
        /// <summary>
        /// Route the incoming <typeparamref name="TRoutingArgs"/> and return # of processed routes.
        /// </summary>
        /// <param name="args">Routing arguments.</param>
        /// <returns># of processed routes.</returns>
        public virtual async Task<int> RouteAsync(TRoutingArgs args)
        {
            var routed = 0;
            foreach (var route in RegisteredRoutes)
            {
                if (!route.IsEligible(args)) continue;
                await route.ProcessAsync(args);
                routed++;
                if (!args.Continue) break;
            }

            return routed;
        }
    }
}
