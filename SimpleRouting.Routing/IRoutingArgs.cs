namespace SimpleRouting.Routing
{
    public interface IRoutingArgs<TIncoming>
    {
        bool Continue { get; set; }
        TIncoming IncomingData { get; set; }
    }
}
