using JsonRpcWebServer.Contract;
using Newtonsoft.Json;

namespace JsonRpcWebServer;

public class GreeterServer: IGreeter
{
    private readonly ILogger<GreeterServer> _logger;

    public GreeterServer(ILogger<GreeterServer> logger)
    {
        _logger = logger;
    }

    public Task<HelloResponse> SayHelloAsync(HelloRequest request , CancellationToken cancellationToken)
    {
        _logger.LogInformation("SayHelloAsync called");
        return Task.FromResult(new HelloResponse { Message = $"Hello {request.Name}" });
    }


    public async Task<IConnectionDetail?> GetConnectionDetailAsync(string factoryName, CancellationToken cancellationToken)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto // Ensures $type is included
        };
        IConnectionDetail connectionDetail = null;
        if (string.Equals(factoryName, "Microsoft.Azure"))
        {
            connectionDetail = new AzureConnectionDetail();
        }
        else if (string.Equals(factoryName, "VMWare.Hypvervisor"))
        {
            connectionDetail = new VmwareConnectionDetail("userName", "password");
        }
        else
        {
            throw new NotImplementedException();
        }

        return await Task.FromResult(connectionDetail);

    }


    public async Task<VMDiskInfo> GetVMDiskInfoAsync(IConnectionDetail connectionDetail, CancellationToken cancellationToken)
    {
        if (connectionDetail is null)
        {
            throw new ArgumentNullException(nameof(connectionDetail));
        }

        if (connectionDetail is AzureConnectionDetail azureConnectionDetail)
        {
            return await Task.FromResult(new VMDiskInfo { DiskSize = 100, DiskType = "SSD", DiskName = "AzureDisk" , DiskPath = $"resourceGroup/azureDisk/{Guid.NewGuid()}" });
        }
        else if (connectionDetail is VmwareConnectionDetail vmwareConnectionDetail)
        {
            return await Task.FromResult(new VMDiskInfo { DiskSize = 200, DiskType = "HDD" ,  DiskName = "VMWareDisk" , DiskPath = $"storage.storage/Cluster.cluster/{Guid.NewGuid()}" });
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public int Add(int a, int b) => a + b;


    // public async Task SentTickAsync( CancellationToken cancellationToken )
    // {
    //     int tickNumer  = 0;
    //     while (!cancellationToken.IsCancellationRequested)
    //     {
    //         await Task.Delay(1000, cancellationToken);
    //         this.OnRequestReceived?.Invoke(this, ++tickNumer);
    //     }
    // }

}
