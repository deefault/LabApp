using System;
using LabApp.Server.Data.Models.Abstractions;
using EntityFrameworkCore.Triggers;

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