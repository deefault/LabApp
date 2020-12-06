using System.IO;
using System.Threading.Tasks;
using SkiaSharp;

namespace LabApp.Server.Services.ImageService
{
    public interface IImageProcessingService
    {
        Task<Stream> GenerateThumbnail(Stream stream, int? logoSideWidth = null);
    }

    public class ImageProcessingService : IImageProcessingService
    {
        public Task<Stream> GenerateThumbnail(Stream stream, int? logoSideWidth = null)
        {
            SKBitmap bitmap = SKBitmap.Decode(stream);
            SKBitmap resized = ThumbnailBitmap(bitmap, logoSideWidth);

            return SaveToStream(resized);
        }

        private static SKBitmap ThumbnailBitmap(SKBitmap bitmap, int? logoSideWidth = null)
        {
            SKSizeI size;
            if (logoSideWidth.HasValue)
            {
                if (bitmap.Width != bitmap.Height)
                {
                    SKRect rect = CalculateCroppingRect(bitmap);
                    SKBitmap croppedBitmap = CropBitmap(bitmap, rect);
                    bitmap = croppedBitmap;
                }

                size = new SKSizeI(bitmap.Height * (logoSideWidth.Value / bitmap.Height), logoSideWidth.Value);
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

        private async Task<Stream> SaveToStream(SKBitmap bm)
        {
            await using Stream stream = new MemoryStream();
            bm.Encode(stream, SKEncodedImageFormat.Jpeg, 100);

            return stream;
        }
    }
}