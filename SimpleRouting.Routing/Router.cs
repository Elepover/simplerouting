using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRouting.Routing
{
    public class Router<TRoutingContext> :
        IRoutable<TRoutingContext>, 
        IEnumerable<IRoutable<TRoutingContext>>
        where TRoutingContext : IRoutingContext
    {
        #region Constructors and operators
        /// <summary>
        /// Get the route at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The route's index.</param>
        public IRoutable<TRoutingContext> this[int index]
            => RegisteredRoutes.ElementAt(index);
        #endregion

        #region Variables
        /// <summary>
        /// Registered routes in this <see cref="Router{TRoutingContext}"/>
        /// </summary>
        public LinkedList<IRoutable<TRoutingContext>> RegisteredRoutes { get; } = new();
        #endregion

        #region Interface implementations
        #region IRoutable implementation

        /// <inheritdoc cref="IRoutable{TRoutingContext}.IsEligible"/>
        public virtual bool IsEligible(TRoutingContext context)
            => RegisteredRoutes.Any(route => route.IsEligible(context));
            

        /// <inheritdoc cref="IRoutable{TRoutingContext}.ProcessAsync"/>
        public virtual Task ProcessAsync(TRoutingContext context)
            => RouteAsync(context);
        #endregion

        #region IEnumerable implementation
        public IEnumerator<IRoutable<TRoutingContext>> GetEnumerator()
            => new RouterEnumerator<IRoutable<TRoutingContext>>(RegisteredRoutes);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
        #endregion

        /// <summary>
        /// Add a new route to the last of the routing queue.
        /// </summary>
        /// <param name="newRoute">New route to add.</param>
        public void Add(IRoutable<TRoutingContext> newRoute)
            => RegisteredRoutes.AddLast(newRoute);
        
        /// <summary>
        /// Route the incoming <typeparamref name="TRoutingContext"/> and return # of processed routes.
        /// </summary>
        /// <param name="context">Routing arguments.</param>
        /// <returns># of processed routes.</returns>
        public virtual async Task<int> RouteAsync(TRoutingContext context)
        {
            var routed = 0;
            foreach (var route in RegisteredRoutes)
            {
                if (!route.IsEligible(context)) continue;
                await route.ProcessAsync(context);
                routed++;
                switch (context.Target)
                {
                    case RouteTarget.Break:
                    {
                        context.Target = RouteTarget.Continue;
                        goto ret;
                    }
                    case RouteTarget.Stop:
                    {
                        context.Target = RouteTarget.Stop;
                        goto ret;
                    }
                    case RouteTarget.Continue: continue;
                }
            }
            ret:
            return routed;
        }
    }
}
