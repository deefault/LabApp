using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using LabApp.Server.Data;
using LabApp.Server.Services.Interfaces;
using LabApp.Server.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.FileProviders;
using SkiaSharp;

namespace LabApp.Server.Services
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
        
        private readonly AppDbContext _db;
        private readonly IFileStorage _fileStorage;

        public ImageService(AppDbContext db, IFileStorage fileStorage)
        {
            _db = db;
            _fileStorage = fileStorage;
        }

        public async Task<(string, string)> SaveImageAsync(IFormFile formFile, bool isLogo = true)
        {
            string ext = await GetImageExtensionAsync(formFile);
            
            Stream stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            
            string savedOriginal = await SaveImageToStorageAsync(stream, $"{Guid.NewGuid()}{ext}");

            SKBitmap bitmap = SKBitmap.Decode(stream); 
            SKBitmap resized = ThumbnailBitmap(bitmap, isLogo);
            string savedThumbnail = await SaveThumbnailToStorageAsync(resized, $"{Guid.NewGuid()}{ext}");
            

            return (savedOriginal, savedThumbnail);
        }

        private async Task<string> GetImageExtensionAsync(IFormFile formFile)
        {
            if (!formFile.ContentType.Contains("image", StringComparison.OrdinalIgnoreCase))
                throw new FileLoadException("not an image");
            string ext = new FileInfo(formFile.FileName).Extension;
            if (string.IsNullOrEmpty(ext) || !PermittedExtensions.Contains(ext))
            {
                throw new ExtensionNotAllowedException(PermittedExtensions);
            }
            // throws exception
            await CheckImageFileSignature(formFile, ext);

            return ext;
        }

        private static async Task CheckImageFileSignature(IFormFile formFile, string ext)
        {
            byte[] signatureBytes = new byte[8];
            await using (Stream stream = formFile.OpenReadStream())
            {
                await stream.ReadAsync(signatureBytes, 0, 8);
            }
            if (!FileSignatureChecker.CheckImage(ext, signatureBytes)) 
                throw new ExtensionNotAllowedException(PermittedExtensions);
            
        }

        private static SKBitmap ThumbnailBitmap(SKBitmap bitmap, bool isLogo)
        {
            SKSizeI size;
            if (isLogo)
            {
                if (bitmap.Width != bitmap.Height)
                {
                    SKRect rect = CalculateCroppingRect(bitmap);
                    SKBitmap croppedBitmap = CropBitmap(bitmap, rect);
                    bitmap = croppedBitmap;
                }
                size = new SKSizeI(bitmap.Height * (LogoSideLength / bitmap.Height), LogoSideLength);
            }
            else size = new SKSizeI(bitmap.Width, bitmap.Height);
            

            return bitmap.Resize(size, SKFilterQuality.Medium);
        }

        private static SKRect CalculateCroppingRect(SKBitmap bitmap)
        {
            SKRect rect;
            if (bitmap.Height > bitmap.Width)
            {
                int side = bitmap.Width;
                rect = new SKRect(0, 0, side, side);
            }
            else
            {
                int side = bitmap.Height;
                float left = (bitmap.Width - side) / 2.0f;
                // квадрат по центру картинки с альбомной ориентацией
                rect = new SKRect(left, 0f, left + side, side);
            }

            return rect;
        }

        private static SKBitmap CropBitmap(SKBitmap bitmap, SKRect rect)
        {
            SKBitmap croppedBitmap = new SKBitmap((int) rect.Width,
                (int) rect.Height);
            SKRect dest = new SKRect(0, 0, rect.Width, rect.Height);
            SKRect source = new SKRect(rect.Left, rect.Top,
                rect.Right, rect.Bottom);

            using (SKCanvas canvas = new SKCanvas(croppedBitmap))
                canvas.DrawBitmap(bitmap, source, dest);
            

            return croppedBitmap;
        }

        private async Task<string> SaveThumbnailToStorageAsync(SKBitmap bm, string filename)
        {
            await using Stream stream = new MemoryStream();
            bm.Encode(stream, SKEncodedImageFormat.Jpeg, 100);
            await _fileStorage.UploadImageThumbnailAsync(stream, filename);

            return filename;
        }
        
        private async Task<string> SaveImageToStorageAsync(Stream stream, string filename)
        {
            await _fileStorage.UploadImageAsync(stream, filename);
            
            return filename;
        }
    }
}