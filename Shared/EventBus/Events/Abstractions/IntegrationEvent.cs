using System;
using LabApp.Shared.EventConsistency.Abstractions;
using Newtonsoft.Json;

namespace LabApp.Shared.EventBus.Events.Abstractions
{
	public class IntegrationEvent : IIntegrationEvent
	{
		public IntegrationEvent()
		{
			Id = Guid.NewGuid();
			CreationDate = DateTime.UtcNow;
		}

		[JsonConstructor]
		public IntegrationEvent(Guid id, DateTime createDate)
		{
			Id = id;
			CreationDate = createDate;
		}

		[JsonProperty]
		public Guid Id { get; set; }

		[JsonProperty]
		public DateTime CreationDate { get; set; }
	}
}