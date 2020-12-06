using System.IO;
using System.Threading.Tasks;
using LabApp.Server.Data.Models.Abstractions;

namespace LabApp.Server.Services.Interfaces
{
    public interface IFileStorage
    {
        /// <returns>Path or Url for file</returns>
        string GetFile(Attachment attachment);
        
        string GetFile(string path);

        /// <returns>Path or Url for file</returns>
        string GetImage(Image image);
        
        string GetImage(string path);
        
        /// <returns>Path or Url for file</returns>
        string GetImageThumbnail(Image image);
        
        string GetImageThumbnail(string path);

        Task<bool> UploadFileAsync(Stream stream, string name);

        Task<bool> UploadImageAsync(Stream stream, string name);
        
        Task<bool> UploadImageThumbnailAsync(Stream stream, string name);
    }
}