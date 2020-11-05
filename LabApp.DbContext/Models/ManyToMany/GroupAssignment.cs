using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabApp.Server.Data.Models.ManyToMany
{
	/// <summary>
	/// Доп информация о задании для конкретной группы
	/// </summary>
	public class GroupAssignment
	{
		public DateTime? Start { get; set; } = DateTime.UtcNow;
		
		public DateTime? DeadLine { get; set; }

		/// <summary>
		/// Для этой группы не нужна данная лабороторная
		/// </summary>
		public bool IsHidden { get; set; } = false;
        
		public int GroupId { get; set; }
        
		public int AssignmentId { get; set; }
        
		[ForeignKey("GroupId")]
		public virtual Group Group { get; set; }
        
		[ForeignKey("AssignmentId")]
		public virtual Assignment Assignment { get; set; }
	}
}