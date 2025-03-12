namespace JsonRpcWebServer.Contract;

public sealed class AzureConnectionDetail: IConnectionDetail
{
    public string FactoryName { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public Guid ConnectionId { get; set; }

    public string ResourceGroup { get; set; }

    public bool IsDisposed { get; private set; }

    public AzureConnectionDetail()
    {
        ConnectionId = Guid.NewGuid();
        FactoryName = "Microsoft.Azure";
        Name = "Azure";
        Address = "https://management.azure.com/";
        ResourceGroup = "Default";
    }

    public override string ToString()
    {
        return $"FactoryName: {FactoryName}, Name: {Name}, Address: {Address}, ConnectionId: {ConnectionId}, ResourceGroup: {ResourceGroup}";
    }

    public event EventHandler? DisposedEvent;
    public void Dispose()
    {
        if (IsDisposed is false)
        {
            DisposedEvent?.Invoke(this, EventArgs.Empty);
            IsDisposed = true;
        }
    }
}


