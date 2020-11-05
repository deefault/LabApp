namespace LabApp.Server.Data.Models.Interfaces
{
    public interface ISoftDeletable
    {
        /// <summary>
        /// Deleted from storage but stored in database
        /// </summary>
        bool IsDeleted { get; set; }
    }
}