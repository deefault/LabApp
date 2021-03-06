using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventBus.Interfaces;
using LabApp.Shared.EventBus.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Utf8Json;

namespace LabApp.Shared.EventBus.Transports.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ISubscriptionsManager _subscriptionsManager;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly string _queueName;
        private readonly int _retryCount;
        private readonly string _brokerName;
        
        private IModel _consumerChannel;

        public RabbitMQEventBus(IConfiguration configuration, IRabbitMQPersistentConnection persistentConnection,
            ILogger<RabbitMQEventBus> logger, ISubscriptionsManager subscriptionsManager, IOptions<RabbitMqOptions> options,
            IServiceScopeFactory scopeFactory)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _subscriptionsManager = subscriptionsManager;
            _scopeFactory = scopeFactory;
            _queueName = options.Value.QueueName;
            _retryCount = 5;
            if (configuration["EventBusRetryCount"] != null)
                _retryCount = int.Parse(configuration["EventBusRetryCount"]);
            _brokerName = options.Value.BrokerName;
            _consumerChannel = CreateConsumerChannel();
        }

        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        _logger.LogWarning(ex,
                            "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id,
                            $"{time.TotalSeconds:n1}", ex.Message);
                    });

            var eventType = @event.GetType();

            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id,
                eventType.Name);

            using (var channel = _persistentConnection.CreateModel())
            {
                _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

                channel.ExchangeDeclare(exchange: _brokerName, type: "direct");
                byte[] body = JsonSerializer.NonGeneric.Serialize(eventType, @event);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

                    channel.BasicPublish(
                        exchange: _brokerName,
                        routingKey: eventType.Name,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subscriptionsManager.GetEventKey<T>();

            if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();

            var hasSubs = _subscriptionsManager.HasSubscriptionsForEvent(eventName);
            if (!hasSubs)
            {
                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(_queueName,
                        exchange: _brokerName,
                        routingKey: eventName);
                }
            }

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subscriptionsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Message_Received;

                var _ = _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Message_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            _logger.LogDebug("Message recieved");
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError(e, "Operation cancelled in Message_Recieved");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        
        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subscriptionsManager.HasSubscriptionsForEvent(eventName))
            {
                using var scope = _scopeFactory.CreateScope();
                Type eventType = _subscriptionsManager.GetEventTypeByName(eventName);
                try
                {
                    var integrationEvent = (IntegrationEvent) JsonSerializer.NonGeneric.Deserialize(eventType, message);

                    var handler = scope.ServiceProvider.GetRequiredService<IIncomingEventHandler>();
                    await handler.HandleAsync(integrationEvent, eventType, eventName);
                }
                catch (JsonParsingException e)
                {
                    _logger.LogError(e, "Error while serializing event to type");
                }
            }
            else
            {
                _logger.LogWarning("No subscription for event {eventName}", eventName);
            }

            _logger.LogTrace("Finished processing RabbitMQ event: {EventName}", eventName);
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _brokerName,
                type: "direct");

            channel.QueueDeclare(queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };
            channel.ModelShutdown += (_, __) => _logger.LogWarning("Shutdown");

            return channel;
        }

        public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subscriptionsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subscriptionsManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();

            _subscriptionsManager.Clear();
        }
    }
}