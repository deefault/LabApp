using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.ManyToMany;

namespace LabApp.Server.Data.QueryModels
{
	public class AssignmentWithGroupInfo
	{
		public Assignment Assignment { get; set; }
		
		public GroupAssignment GroupAssignment { get; set; }
	}
}