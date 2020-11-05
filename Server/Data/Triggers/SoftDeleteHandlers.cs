using EntityFrameworkCore.Triggers;
using LabApp.Server.Data.Models.Interfaces;

namespace LabApp.Server.Data.Triggers
{
	public static class SoftDeleteHandlers
	{
		static SoftDeleteHandlers()
		{
			Triggers<ISoftDeletable>.Deleting += entry =>
			{
				entry.Entity.IsDeleted = true;
				entry.Cancel = true;
			};
		} 
	}
}