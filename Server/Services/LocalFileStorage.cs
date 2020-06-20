using System;
using System.IO;
using System.Threading.Tasks;
using LabApp.Server.Data.Models.Abstractions;
using LabApp.Server.Services.Interfaces;

namespace LabApp.Server.Services
{
    public class LocalFileStorage : IFileStorage
    {
        private static readonly string AttachmentPath = GetPath("attachments");
        private static readonly string ImagePath = GetPath("images");
        private static readonly string ImageThumbnailPath = GetPath("thumbnails");


        public string GetFile(Attachment attachment)
        {
            return GetFile(attachment.Url);
        }

        public string GetFile(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? null : Path.Combine(AttachmentPath, path);
        }

        public string GetImage(Image image)
        {
            return GetImage(image.Url);
        }

        public string GetImage(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? null : Path.Combine(ImagePath, path);
        }

        public string GetImageThumbnail(Image image)
        {
            return GetImageThumbnail(image.ThumbnailUrl);
        }

        public string GetImageThumbnail(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? null : Path.Combine(ImageThumbnailPath, path);
        }

        public async Task<bool> UploadFileAsync(Stream stream, string name) =>
            await UploadAsync(stream, Path.Combine(AttachmentPath, name));

        public async Task<bool> UploadImageAsync(Stream stream, string name) =>
            await UploadAsync(stream, Path.Combine(ImagePath, name));

        public async Task<bool> UploadImageThumbnailAsync(Stream stream, string name) =>
            await UploadAsync(stream, Path.Combine(ImageThumbnailPath, name));

        private async Task<bool> UploadAsync(Stream stream, string path)
        {
            if (File.Exists(path)) return false;
            await using FileStream fs = new FileStream(path, FileMode.CreateNew);
            await stream.CopyToAsync(fs);

            return true;
        }

        private static string GetPath(string path) => Path.Combine(AppConfiguration.LocalStoragePath, path);
    }
}