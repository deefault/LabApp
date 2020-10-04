using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.EventBus
{
	public class SubscriptionsManager : ISubscriptionsManager
	{
		private readonly Dictionary<string, List<Type>> _eventHandlers =
			new Dictionary<string, List<Type>>();

		private readonly List<Type> _eventTypes = new List<Type>();

		public bool IsEmpty => _eventHandlers.Keys.Count > 0;

		public event EventHandler<string> OnEventRemoved;

		public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
		{
			var eventName = GetEventKey<T>();
			if (!HasSubscriptionsForEvent(eventName))
			{
				_eventHandlers.Add(eventName, new List<Type> {typeof(TH)});
				_eventTypes.Add(typeof(T));
			}
			else
			{
				var handlers = _eventHandlers[eventName];
				Type handlerType = typeof(TH);
				if (handlers.Any(x => x == handlerType))
				{
					throw new ArgumentException($"Event handler {handlerType.Name} for event {eventName}");
				}

				handlers.Add(handlerType);
			}
		}

		public void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
		{
			var eventName = GetEventKey<T>();
			if (!HasSubscriptionsForEvent(eventName)) return;

			var handler = _eventHandlers[eventName].SingleOrDefault(x => x == typeof(TH));
			_eventHandlers[eventName].Remove(handler);
			if (!_eventHandlers.Any())
			{
				_eventHandlers.Remove(eventName);
				var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
				if (eventType != null)
				{
					_eventTypes.Remove(eventType);
				}
			}
		}

		public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent =>
			_eventHandlers.ContainsKey(GetEventKey<T>());

		public bool HasSubscriptionsForEvent(string eventName) =>
			_eventHandlers.ContainsKey(eventName);
		
		public Type GetEventTypeByName(string eventName)
		{
			var type = _eventTypes.FirstOrDefault(x => x.Name == eventName);
			if (type == null) throw new Exception("Cannot find event type by name");

			return type;
		}

		public void Clear() => _eventHandlers.Clear();

		public IEnumerable<Type> GetHandlersForEvent<T>()
			where T : IntegrationEvent
		{
			var eventName = GetEventKey<T>();
			return GetHandlersForEvent(eventName);
		}

		public IEnumerable<Type> GetHandlersForEvent(string eventName)
		{
			return _eventHandlers[eventName];
		}

		public string GetEventKey<T>()
		{
			return typeof(T).Name;
		}
	}
}