using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LabApp.Shared.Enums;

namespace LabApp.Shared.Dto.Teacher
{
	public class StudentAssignmentDto
	{
		public int Id { get; set; }

		public int AssignmentId { get; set; }
        
		public int GroupId { get; set; }

		public int StudentId { get; set; }

		public AssignmentStatus Status { get; set; }

		public decimal? Score { get; set; }

		/// <summary>
		/// штраф
		/// </summary>
		public decimal? FineScore { get; set; }

		public DateTime? Submitted { get; set; }
        
		public DateTime? LastStatusChange { get; set; }
        
		public DateTime? Completed { get; set; }
		

		public UserListDto Student { get; set; }

		public string GroupName { get; set; }
		
		public string AssignmentName { get; set; }
		
		
	}
	
	
	public class StudentAssignmentDetailDto : StudentAssignmentDto
	{
		public string Body { get; set; }
        
		public string TeacherComment { get; set; }

		public virtual IEnumerable<AttachmentDto> Attachments { get; set; }
	}	
}