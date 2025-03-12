using StreamJsonRpc;
using System.Text.Json.Serialization;

namespace JsonRpcWebServer.Contract;


//[JsonDerivedType(typeof(AzureConnectionDetail),"AzureConnectionDetail")]
//[JsonDerivedType(typeof(VmwareConnectionDetail),"VMWareConnectionDetail")]

public interface IConnectionDetail : IDisposable
{
    string FactoryName { get; set; }

    string Name { get; set; }

    string Address { get; set; }

    Guid ConnectionId { get; set; }

    abstract string ToString();
}
