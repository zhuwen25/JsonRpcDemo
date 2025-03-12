// See https://aka.ms/new-console-template for more information

using JsonRpcWebServer.CommonJsonRpc;
using JsonRpcWebServer.Contract;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;
using StreamJsonRpc;
using System.Net.WebSockets;

namespace JsonRpcWebSocket;

class Program
{
    static async Task Main(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            Console.WriteLine("Canceling...");
            cancellationTokenSource.Cancel();
            e.Cancel = true;
        };

        try
        {
           // while (cancellationTokenSource.IsCancellationRequested == false)
            {
                Console.WriteLine("Starting server, Press Ctrl+C to end...");
                await MainAsync(cancellationTokenSource.Token);
            }

        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine("The operation was canceled.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    static async Task MainAsync(CancellationToken cancellationToken)
    {
        using var webSocket = new ClientWebSocket();
        try
        {
            using var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            await webSocket.ConnectAsync(new Uri("wss://localhost:5000/rpc/greeter"), cancellationToken);


            if (webSocket.State == WebSocketState.Open)
            {
                var wsJsonRpc = new WsJsonRpc();
                var greetClient = await wsJsonRpc.AttachService<IGreeter>(webSocket);

                var connectionDetail = await greetClient.GetConnectionDetailAsync("VMWare.Hypvervisor",cancellationToken);
                Console.WriteLine("Connection detail: " + connectionDetail?.ToString());

                //Try using interface to get VMDiskInfo
                // for Azure
                var azureConnection = new AzureConnectionDetail();

                var azVmDisk  = await greetClient.GetVMDiskInfoAsync(azureConnection, cancellationToken);
                Console.WriteLine($"azVmDisk: {azVmDisk}");
                Console.WriteLine("--------------------------------------");


                var vmWareConnection = new VmwareConnectionDetail("Yongwen","YzPassword");

                var vmwareDisk = await greetClient.GetVMDiskInfoAsync(vmWareConnection, cancellationToken);
                Console.WriteLine($"vmwareDisk: {vmwareDisk}");
                Console.WriteLine("--------------------------------------");


            }

            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close from client", CancellationToken.None);
            Console.WriteLine("Connection was closed.");

        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine("The operation was canceled.");
        }
        catch (WebSocketException e)
        {
            Console.WriteLine($"WebSocketException {e}");
        }
        catch (ObjectDisposedException e)
        {
            Console.WriteLine($"ObjectDisposedException {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception {e}");
            // ignored
        }

    }


    static async Task MainAsync1(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting connecting to web socket...");

        using (var socket = new  ClientWebSocket() )
        {
            try
            {
                using var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                await socket.ConnectAsync(new Uri("wss://localhost:5000/rpc/greeter"), cancellationToken);

                var messageFormatter = new JsonMessageFormatter
                {
                    JsonSerializer = { TypeNameHandling = TypeNameHandling.All,TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple}

                };

                var greeterClient = JsonRpc.Attach<IGreeter>( new WebSocketMessageHandler(socket,messageFormatter) );
                var greeterReq = new HelloRequest() { Name = "Yongwen Json Rpc client" };
                var resp = await greeterClient.SayHelloAsync(greeterReq, cancellationToken);
                Console.WriteLine("Greeter response: " + resp?.Message);

                //Try to find AzureVmware

                var connectionDetail = await greeterClient.GetConnectionDetailAsync("VMWare.Hypvervisor",cancellationToken);

                var connectionDetail2 = JsonConvert.DeserializeObject<IConnectionDetail>(
                    JsonConvert.SerializeObject(connectionDetail),
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }
                );

                Console.WriteLine("Connection detail: " + connectionDetail.ToString());

                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close from client", CancellationToken.None);
                Console.WriteLine("Connection was closed.");

            }
            catch (OperationCanceledException e)
            {
                //Closing is initiated by Ctrl+C on the client
                //Close the web socket gracefully -- before JsonRpc is disposed to avoid the socket going into aborted state.
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


    }
}
