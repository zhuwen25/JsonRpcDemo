using Newtonsoft.Json;

namespace JsonRpcWebServer.Contract;

public interface IGreeter
{
   // Task<HelloResponse> SayHelloAsync(HelloRequest request, CancellationToken cancellationToken);
    Task<HelloResponse> SayHelloAsync(HelloRequest request, CancellationToken cancellationToken);
    // Task<int> AddAsync(int a, int b, CancellationToken cancellationToken);
    // Task SentTickAsync(CancellationToken cancellationToken);
    // event EventHandler<int> OnRequestReceived;
    Task<IConnectionDetail?> GetConnectionDetailAsync(string factoryName, CancellationToken cancellationToken);

    Task<VMDiskInfo> GetVMDiskInfoAsync(IConnectionDetail connectionDetail , CancellationToken cancellationToken);

}
