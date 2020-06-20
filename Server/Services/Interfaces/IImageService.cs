using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LabApp.Server.Services.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="isLogo"> Make a square thumbnail</param>
        /// <returns>(image path, thumbnail path or null)</returns>
        Task<(string, string)> SaveImageAsync(IFormFile formFile, bool isLogo = true);
    }
}