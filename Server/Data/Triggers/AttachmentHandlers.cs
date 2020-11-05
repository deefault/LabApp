using System;
using EntityFrameworkCore.Triggers;
using LabApp.Server.Data.Models.Interfaces;

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