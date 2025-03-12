using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StreamJsonRpc;
using System.Net.WebSockets;

namespace JsonRpcWebServer.CommonJsonRpc;

/// <summary>
/// JsonRpc base web socket
/// </summary>
public class WsJsonRpc
{
    private readonly ILogger _logger;
    private HclJsonRpc _hclJsonRpc;
    private IJsonRpcMessageHandler _handler;
    private CancellationToken _cancellationToken;
    private WebSocket _webSocket;

    public Task Completion
    {
        get
        {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
            return _hclJsonRpc.Completion;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
        }
    }

    public WsJsonRpc( ILogger logger = null , CancellationToken cancellation= default) {
        _logger = logger;
        _cancellationToken = cancellation;
    }

    public async Task<T> AttachService<T>(WebSocket wss ,object? target =null ) where T : class
    {
        _webSocket = wss;
        var messageHandler = new WebSocketMessageHandler(wss,new JsonMessageFormatter()
        {
            JsonSerializer =
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
               // Converters = { new () } // Add custom converters
            }
        });
        _hclJsonRpc = new HclJsonRpc(messageHandler,target);
        var service = _hclJsonRpc.Attach<T>();
        _hclJsonRpc.StartListening();

        return await Task.FromResult(service);

       // _hclJsonRpc.StartListening();

        // using (var jsonRpc = new JsonRpc( new WebSocketMessageHandler(socket , messageFormatter ), _greeterServer ))
        // {
        //
        //     jsonRpc.CancelLocallyInvokedMethodsWhenConnectionIsClosed = true;
        //     jsonRpc.StartListening();
        //     await jsonRpc.Completion;
        //     _logger.LogInformation("Greeter client disconnected");
        // }
        //
        // return new EmptyResult();
    }

}
