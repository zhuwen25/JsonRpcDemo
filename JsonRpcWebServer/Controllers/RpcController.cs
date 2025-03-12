using JsonRpcWebServer.CommonJsonRpc;
using JsonRpcWebServer.Contract;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StreamJsonRpc;


namespace JsonRpcWebServer.Controllers;


[ApiController]
[Route("[controller]")]
public class RpcController : ControllerBase
{
    private readonly ILogger<RpcController> _logger;
    private readonly GreeterServer _greeterServer;

    public RpcController(ILogger<RpcController> logger, GreeterServer greeterServer)
    {
        _logger = logger;
        _greeterServer = greeterServer;
    }

    [Route("/rpc/greeter")]
    public async Task<IActionResult> Greeter()
    {
        if (!this.HttpContext.WebSockets.IsWebSocketRequest)
        {
            return new BadRequestResult();
        }
        var socket = await this.HttpContext.WebSockets.AcceptWebSocketAsync();

        var rpc = new WsJsonRpc(_logger);
        await rpc.AttachService<IGreeter>(socket ,_greeterServer );

        await rpc.Completion;
        return new EmptyResult();
    }


}
