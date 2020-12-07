using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.RabbitMQ;
using LabApp.Shared.Infrastructure.Options;
using LabApp.Shared.RpcClient.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace LabApp.Shared.RpcClient
{
    public class RabbitMQRpcClient : IRpcClient, IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<RabbitMQRpcClient> _logger;

        private readonly AsyncCollection<ResponseData> _respQueue = new AsyncCollection<ResponseData>();

        private AsyncEventingBasicConsumer _consumer;
        private IModel _channel;
        
        private readonly int _retryCount;
        private readonly string _brokerName;

        public RabbitMQRpcClient(
            IRabbitMQPersistentConnection persistentConnection,
            ILogger<RabbitMQRpcClient> logger,
            IOptions<RabbitMqOptions> options,
            IConfiguration configuration)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _retryCount = 5;
            if (configuration["EventBusRetryCount"] != null)
                _retryCount = int.Parse(configuration["EventBusRetryCount"]);
            _brokerName = options.Value.BrokerName;
            CreateConsumer();
        }

        private void CreateConsumer()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            _channel = _persistentConnection.CreateModel();
            _channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _channel.Dispose();
                CreateConsumer();
            };
            _channel.ModelShutdown += (_, __) => _logger.LogWarning("Shutdown");
            _consumer = new AsyncEventingBasicConsumer(_channel);

            _consumer.Received += OnEventRecieved;
        }

        private async Task OnEventRecieved(object sender, BasicDeliverEventArgs ea)
        {
            await _respQueue.AddAsync(new ResponseData(ea.BasicProperties.CorrelationId, ea.Body));

            _channel.BasicAck(ea.DeliveryTag, false);
        }

        public Task<TResponse> Send<TResponse>(IRpcRequest<TResponse> request)
            where TResponse : IRpcResponse
            => Send<TResponse>(request, CancellationToken.None);

        public async Task<TResponse> Send<TResponse>(IRpcRequest<TResponse> request, CancellationToken ct)
            where TResponse : IRpcResponse
        {
            var correlationId = SendInternal(request);

            var res = await _respQueue.TakeAsync(ct);

            if (correlationId != res.CorrelationId) throw new ArgumentOutOfRangeException(correlationId);
            TResponse data = Utf8Json.JsonSerializer.Deserialize<TResponse>(res.Data.ToArray());

            return data;
        }

        public Task Send(IRpcRequest request) => Send(request, CancellationToken.None);

        public async Task Send(IRpcRequest request, CancellationToken ct)
        {
            var correlationId = SendInternal(request);

            var res = await _respQueue.TakeAsync(ct);
            if (correlationId != res.CorrelationId) throw new ArgumentOutOfRangeException(correlationId);
        }

        private string SendInternal(IRpcRequest request)
        {
            Policy policy = GetSendPolicy(request.CorrelationId);

            IBasicProperties props = CreateProperties(request, _channel);
            _channel.ExchangeDeclare(exchange: _brokerName, type: "direct");


            ReadOnlyMemory<byte> bytes = Utf8Json.JsonSerializer.Serialize(request);


            var routingKey = request.GetType().Name;
            policy.Execute(() => _channel.BasicPublish(exchange: _brokerName,
                routingKey: routingKey,
                basicProperties: props,
                body: bytes));
            
            _channel.BasicConsume(_consumer, queue: props.ReplyTo, autoAck: false);

            return props.CorrelationId;
        }

        private IBasicProperties CreateProperties(IRpcRequest request, IModel channel)
        {
            var props = channel.CreateBasicProperties();
            props.ReplyTo = GenerateReplyToName(request);
            props.DeliveryMode = 2;
            props.ContentType = "application/json";
            props.CorrelationId = request.CorrelationId.ToString();

            return props;
        }

        private string GenerateReplyToName(IRpcRequest request)
        {
            return $"{request.CorrelationId.ToString()}_reply";
        }

        private RetryPolicy GetSendPolicy(Guid correlationId)
        {
            return Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        _logger.LogWarning(ex,
                            "Could not publish event: {CorrelationId} after {Timeout}s ({ExceptionMessage})",
                            correlationId,
                            $"{time.TotalSeconds:n1}", ex.Message);
                    });
        }

        public void Dispose()
        {
            _persistentConnection?.Dispose();
            _channel?.Dispose();
        }
    }
}