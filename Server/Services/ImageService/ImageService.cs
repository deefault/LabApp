using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Utilities;

namespace LabApp.Server.Services.ImageService
{
    public class ImageServiceException : Exception
    {
        protected ImageServiceException(string msg) : base(msg)
        {
        }
    }

    public class ExtensionNotAllowedException : ImageServiceException
    {
        public ExtensionNotAllowedException(string msg, string[] permittedExtensions) : base(msg)
        {
            PermittedExtensions = permittedExtensions;
        }

        public ExtensionNotAllowedException(string[] permittedExtensions)
            : this($"Allowed file extensions: {string.Join(", ", permittedExtensions)}", permittedExtensions)
        {
        }

        string[] PermittedExtensions { get; }
    }

    public class ImageService : IImageService
    {
        private static readonly string[] PermittedExtensions = {".jpg", ".png", ".jpeg"};
        private const int LogoSideLength = 50;

        private readonly IFileStorage _fileStorage;
        private readonly IImageProcessingService _imageProcessingService;

        public ImageService(IFileStorage fileStorage, IImageProcessingService imageProcessingService)
        {
            _fileStorage = fileStorage;
            _imageProcessingService = imageProcessingService;
        }

        public async Task<(string, string)> SaveImageAsync(Stream inputStream, string filename, bool isLogo = true)
        {
            string ext = await GetImageExtensionAsync(inputStream, filename);

            string savedOriginal = await SaveImageToStorageAsync(inputStream, $"{Guid.NewGuid()}{ext}");

            // generate and save thumbnail
            Stream stream = new MemoryStream();
            await inputStream.CopyToAsync(stream);
            var resized =
                await _imageProcessingService.GenerateThumbnail(stream, isLogo ? LogoSideLength : (int?) null);
            string savedThumbnail = await SaveImageToStorageAsync(resized, filename);

            return (savedOriginal, savedThumbnail);
        }

        private async Task<string> GetImageExtensionAsync(Stream inputStream, string filename)
        {
            string ext = new FileInfo(filename).Extension;
            if (string.IsNullOrEmpty(ext) || !PermittedExtensions.Contains(ext))
            {
                throw new ExtensionNotAllowedException(PermittedExtensions);
            }

            // throws exception
            await CheckImageFileSignature(inputStream, ext);

            return ext;
        }

        private static async Task CheckImageFileSignature(Stream stream, string ext)
        {
            byte[] signatureBytes = new byte[8];

            await stream.ReadAsync(signatureBytes, 0, 8);

            if (!FileSignatureChecker.CheckImage(ext, signatureBytes))
                throw new ExtensionNotAllowedException(PermittedExtensions);
            stream.Position = 0;
        }

        private async Task<string> SaveImageToStorageAsync(Stream stream, string filename)
        {
            await _fileStorage.UploadImageAsync(stream, filename);

            return filename;
        }
    }
}