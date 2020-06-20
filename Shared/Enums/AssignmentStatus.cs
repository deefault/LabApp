using System;

namespace LabApp.Shared.Enums
{
	[Flags]
	public enum AssignmentStatus : byte
	{
		Submitted = 0,
		ChangesRequested = 1,
		NeedReview = 2,
		Approved = 3,
	}
}