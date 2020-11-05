using System;

namespace LabApp.Server.Data.Models.Interfaces
{
    /// <summary>
    /// Отложенное удаление (хз как на английском)
    /// </summary>
    public interface IJobDeleted
    {
        /// <summary>
        /// Deleted from storage but stored in database
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Delete on next job run or next run after DeleteDate (if it is not null)
        /// </summary>
        bool ToDelete { get; set; }
        
        /// <summary>
        ///  Delete after this date
        /// </summary>
        DateTime? DeleteDate { get; set; }
    }
}