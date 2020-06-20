using System;
using System.Collections.Generic;
using LabApp.Shared.Enums;

namespace LabApp.Shared.Dto
{
	public class ConversationDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public ConversationType Type { get; set; }

		public IEnumerable<UserListDto> Users { get; set; }

		public DateTime Inserted { get; set; } = DateTime.UtcNow;
	}
}