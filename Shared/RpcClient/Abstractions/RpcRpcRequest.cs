using System;

namespace LabApp.Shared.RpcClient.Abstractions
{
    public class RpcRpcRequest: IRpcRequest
    {
        public Guid CorrelationId { get; }

        public RpcRpcRequest()
        {
            CorrelationId = Guid.NewGuid();
        }
    }
    
    public class RpcRpcRequest<TResponse> : IRpcRequest<TResponse>
        where TResponse: IRpcResponse
    {
        public Guid CorrelationId { get; }

        public RpcRpcRequest()
        {
            CorrelationId = Guid.NewGuid();
        }
    }
}