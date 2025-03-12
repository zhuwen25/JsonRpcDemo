using StreamJsonRpc;
using StreamJsonRpc.Protocol;
using StreamJsonRpc.Reflection;

namespace JsonRpcWebServer.CommonJsonRpc;

public class HclJsonRpc : JsonRpc , IJsonRpcTracingCallbacks
{
    public HclJsonRpc(IJsonRpcMessageHandler messageHandler,object? target=null ) : base(messageHandler,target)
    {

    }

    protected override ValueTask SendAsync(JsonRpcMessage message, CancellationToken cancellationToken)
    {
        return base.SendAsync(message, cancellationToken);
    }

    protected override ValueTask<JsonRpcMessage> DispatchRequestAsync(JsonRpcRequest request, TargetMethod targetMethod, CancellationToken cancellationToken)
    {
        return base.DispatchRequestAsync(request, targetMethod, cancellationToken);
    }

    void IJsonRpcTracingCallbacks.OnMessageDeserialized(JsonRpcMessage message, object encodedMessage)
    {
        Console.WriteLine($"OnMessageDeserialized message:{encodedMessage}" );
    }
    void IJsonRpcTracingCallbacks.OnMessageSerialized(JsonRpcMessage message, object encodedMessage)
    {
        Console.WriteLine($"OnMessageSerialized message:{message}" );
    }

    protected override Type? GetErrorDetailsDataType(JsonRpcError error)
    {
        var ret = base.GetErrorDetailsDataType(error);
        if (ret != typeof(Exception)) return ret;
        // if (error.Error?.Data is not null && error.Error?.Data?.ToString().IndexOf(Exception., StringComparison.Ordinal) > -1)
        // {
        //     return typeof(FaultException);
        // }

        return ret;
    }
}
