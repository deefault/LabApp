using System;
using System.Collections.Generic;

namespace LabApp.Shared.Dto
{
	public class MessageDto
	{
		public int Id { get; set; }
        
		public string Text { get; set; }
        
		public int UserId { get; set; }
        
		public DateTime Sent { get; set; } = DateTime.UtcNow;
        
		public int ConversationId { get; set; }
		

		public virtual IEnumerable<AttachmentDto> Attachments { get; set; }
	}
}