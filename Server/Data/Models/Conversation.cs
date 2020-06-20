using System;
using System.Collections.Generic;
using LabApp.Server.Data.Models.Abstractions;
using LabApp.Shared.Enums;
using LabApp.Server.Data.Models.ManyToMany;

namespace LabApp.Server.Data.Models
{
    public class Conversation : IInsertedTrackable
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int? AssignmentId { get; set; }

        public ConversationType Type { get; set; }


        public virtual IEnumerable<Message> Messages { get; set; }

        public virtual IEnumerable<UserConversation> Users { get; set; }
        
        public virtual StudentAssignment Assignment { get; set; }

        public DateTime Inserted { get; set; } = DateTime.UtcNow;
    }
}