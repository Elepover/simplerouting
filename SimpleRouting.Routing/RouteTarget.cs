namespace SimpleRouting.Routing
{
    /// <summary>
    /// Decides which target to be routed next.
    /// </summary>
    public enum RouteTarget
    {
        /// <summary>
        /// Route to next route.
        /// </summary>
        Continue,
        /// <summary>
        /// Skip remaining routes and route to next upper level route.
        /// </summary>
        Break,
        /// <summary>
        /// Stop all routing.
        /// </summary>
        Stop
    }
}