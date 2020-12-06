using System.IO;
using System.Threading.Tasks;

namespace LabApp.Server.Services.ImageService
{
    public interface IImageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="isLogo"> Make a square thumbnail</param>
        /// <param name="filename">Filename with ext</param>
        /// <returns>(image path, thumbnail path or null)</returns>
        /// <exception cref="ExtensionNotAllowedException">If image extension is not allowed for image</exception>
        Task<(string, string)> SaveImageAsync(Stream inputStream, string filename, bool isLogo = true);
    }
}