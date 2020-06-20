using System.Collections.Generic;

namespace LabApp.Shared.Dto.Teacher
{
	public class GroupTableEntryDto
	{
		public UserListDto Student { get; set; }
		
		public IEnumerable<StudentAssignmentDto> StudentAssignments { get; set; }
	}
	
	
	public class GroupTableDto
	{
		public IEnumerable<AssignmentDto> Assignments { get; set; }
		public IEnumerable<GroupTableEntryDto> Entries { get; set; }
	}
}