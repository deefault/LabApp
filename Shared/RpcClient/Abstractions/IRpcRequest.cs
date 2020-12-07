using System;

namespace LabApp.Shared.RpcClient.Abstractions
{
    public interface IRpcRequest
    {
        public Guid CorrelationId { get; }
    }
    
    public interface IRpcRequest<TResponse> : IRpcRequest
        where TResponse: IRpcResponse
    {
    }
}