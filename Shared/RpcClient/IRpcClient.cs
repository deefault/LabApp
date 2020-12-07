using System.Threading;
using System.Threading.Tasks;
using LabApp.Shared.RpcClient.Abstractions;

namespace LabApp.Shared.RpcClient
{
    public interface IRpcClient
    {
        Task<TResponse> Send<TResponse>(IRpcRequest<TResponse> request) 
            where TResponse : IRpcResponse;
        Task<TResponse> Send<TResponse>(IRpcRequest<TResponse> request, CancellationToken ct) 
            where TResponse : IRpcResponse;
        Task Send(IRpcRequest request);
        Task Send(IRpcRequest request, CancellationToken ct);
    }
}