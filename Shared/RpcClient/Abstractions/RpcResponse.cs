using System;

namespace LabApp.Shared.RpcClient.Abstractions
{
    public abstract class RpcResponse<TResponse> : IRpcResponse
    {
        public Guid CorrelationId { get; }

        protected RpcResponse(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}