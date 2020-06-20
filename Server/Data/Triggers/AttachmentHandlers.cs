using System;
using LabApp.Server.Data.Models.Abstractions;
using EntityFrameworkCore.Triggers;

namespace LabApp.Server.Data.Triggers
{
    public static class AttachmentHandlers
    {
        static AttachmentHandlers()
        {
            Triggers<IJobDeleted>.Deleting += entry =>
            {
                entry.Entity.DeleteDate = DateTime.UtcNow;
                entry.Entity.ToDelete = true;
                entry.Cancel = true;
            };
            
            Triggers<IInsertedTrackable>.Inserted += entry =>
            {
                entry.Entity.Inserted = DateTime.UtcNow;
            };
        }
    }
}