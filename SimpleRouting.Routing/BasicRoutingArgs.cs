namespace SimpleRouting.Routing
{
    public class BasicRoutingArgs<TIncoming> : IRoutingArgs<TIncoming>
    {
        public BasicRoutingArgs(TIncoming incomingData)
        {
            IncomingData = incomingData;
        }

        public bool Continue { get; set; } = false;

        /// <summary>
        /// Incoming data.
        /// </summary>
        public TIncoming IncomingData { get; set; }
    }
}
