using System;

namespace LabApp.Server.Data.Models
{
	public enum NotificationType
	{
		// common
		Common,
		System,
		// student
		AssignmentApproved,
		AssignmentChangesRequested,

		// teacher
		AssignmentSubmitted,
		AssignmentNeedReview
		
	}
	
	//TODO add to context
	public abstract class Notification
	{
		public DateTime DateTime { get; set; } = DateTime.UtcNow;

		public string Title { get; set; }

		public string Text { get; set; }

		public bool Read { get; set; } = false;
		
		public int UserId { get; set; }
		
		public NotificationType Type { get; set; }
		
		public virtual User User { get; set; }
	}
	
	public class CommonNotification : Notification
	{
	}
	
	public class SystemNotification : Notification
	{
	}

	public class AssignmentApprovedNotification : Notification
	{
		public int StudentAssignmentId { get; set; }
		
		public virtual StudentAssignment StudentAssignment { get; set; }
	}
	
	public class AssignmentChangesRequestedNotification : Notification
	{
		public int StudentAssignmentId { get; set; }
		
		public virtual StudentAssignment StudentAssignment { get; set; }
	}
	
	public class AssignmentSubmittedNotification : Notification
	{
		public int StudentAssignmentId { get; set; }
		
		public virtual StudentAssignment StudentAssignment { get; set; }
	}
	
	public class AssignmentNeedReviewNotification : Notification
	{
		public int StudentAssignmentId { get; set; }
		
		public virtual StudentAssignment StudentAssignment { get; set; }
	}
}