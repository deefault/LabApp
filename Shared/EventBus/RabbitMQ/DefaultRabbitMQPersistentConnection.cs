using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using RabbitMQ.Client.Core.DependencyInjection.Models;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace LabApp.Shared.EventBus.RabbitMQ
{
	public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
	{
		private readonly ConnectionFactory _connectionFactory;
		private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;
		private readonly RabbitMqConnectionOptionsContainer _optionsContainer;

		private IConnection _connection;
		private readonly int _retryCount;
		private bool _disposed = false;

		readonly object sync = new object();

		private RabbitMqClientOptions Options => _optionsContainer.Options.ConsumerOptions;

		public DefaultRabbitMQPersistentConnection(ConnectionFactory connectionFactory, IConfiguration configuration,
			ILogger<DefaultRabbitMQPersistentConnection> logger, RabbitMqConnectionOptionsContainer optionsContainer)
		{
			_connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
			_logger = logger;
			_optionsContainer = optionsContainer;
			_retryCount = string.IsNullOrEmpty(configuration["EventBusRetryCount"])
				? 5
				: int.Parse(configuration["EventBusRetryCount"]);
		}

		public bool IsConnected =>_connection != null && _connection.IsOpen && !_disposed;
		

		public bool TryConnect()
		{
			if (!IsConnected)
			{
				lock (sync)
				{
					if (!IsConnected)
					{
						_logger.LogInformation("RabbitMQ Client is trying to connect...");
						var policy = Policy.Handle<SocketException>()
							.Or<BrokerUnreachableException>()
							.WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
								(ex, time) =>
								{
									_logger.LogWarning(ex,
										"RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})",
										$"{time.TotalSeconds:n1}", ex.Message);
								}
							);
						Options.AutomaticRecoveryEnabled = true;
						Options.RequestedConnectionTimeout = TimeSpan.FromSeconds(10);
						policy.Execute(() => _connection = _connectionFactory.CreateConnection());
						
						if (IsConnected)
						{
							_logger.LogInformation("RabbitMQ Client connected");
							_connection.ConnectionShutdown += OnShutdown;
							_connection.CallbackException += OnCallbackException;
							_connection.ConnectionBlocked += OnConnectionBlocked;
						}
						else
						{
							_logger.LogCritical("Could not connect");
						}
					}
				}
			}

			return false;
		}

		public IModel CreateModel()
		{
			if (!IsConnected)
				throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");

			return _connection.CreateModel();
		}

		private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
		{
			if (_disposed) return;

			_logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

			TryConnect();
		}

		private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
		{
			if (_disposed) return;
			_logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect... {ex}", e.Exception.Message);

			TryConnect();
		}

		private void OnShutdown(object sender, ShutdownEventArgs args)
		{
			if (_disposed) return;

			_logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

			TryConnect();
		}

		public void Dispose()
		{
			_logger.LogDebug("Disposing persistent connection");
			if (_disposed) return;

			_disposed = true;

			try
			{
				_connection.Dispose();
			}
			catch (IOException ex)
			{
				_logger.LogCritical(ex.ToString());
			}
		}
	}
}