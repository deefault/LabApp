using System;

namespace LabApp.Server.Data.Models.Abstractions
{
    public interface IInsertedTrackable
    {
        public DateTime Inserted { get; set; }
    }
}