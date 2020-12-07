using System;

namespace LabApp.Shared.RpcClient
{
    internal class ResponseData
    {
        public string CorrelationId { get; }

        public ReadOnlyMemory<byte> Data { get; }

        public ResponseData(string correlationId, ReadOnlyMemory<byte> data)
        {
            CorrelationId = correlationId;
            Data = data;
        }
    }
}