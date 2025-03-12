namespace JsonRpcWebServer.Contract;

public class VmwareConnectionDetail : IConnectionDetail
{
    public string FactoryName { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public Guid ConnectionId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public bool IsDisposed { get; private set; }
    public VmwareConnectionDetail( string username, string password )
    {
        ConnectionId = Guid.NewGuid();
        FactoryName = "VMWare.Hypvervisor";
        Name = "Vsphere";
        Address = "https://192.163.0.293/sdk";
        Username = username;
        Password = password;
    }

    public override string ToString()
    {
        return $"FactoryName: {FactoryName}, Name: {Name}, Address: {Address}, ConnectionId: {ConnectionId}, Username: {Username}, Password: {Password}";
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
