using System;

namespace LabApp.Shared.RpcClient.Abstractions
{
    public interface IRpcResponse
    {
        public Guid CorrelationId { get; }
    }
}