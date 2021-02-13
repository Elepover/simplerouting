namespace SimpleRouting.Routing.Extensions
{
    public static class RoutingContextExtensions
    {
        /// <summary>
        /// Sets flag to: route to next route.
        /// </summary>
        public static void Continue(this IRoutingContext context) => context.Target = RouteTarget.Continue;
        /// <summary>
        /// Sets flag to: skip remaining routes and route to next upper level route.
        /// </summary>
        public static void Break(this IRoutingContext context) => context.Target = RouteTarget.Break;
        /// <summary>
        /// Sets flag to: stop all routing.
        /// </summary>
        public static void Stop(this IRoutingContext context) => context.Target = RouteTarget.Stop;
    }
}