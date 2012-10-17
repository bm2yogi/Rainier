using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Rainier.Images
{
    public interface IImageFactory
    {
        Stream GetImage(int height = 300, int width = 400, string query = "water");
    }

    public class ImageFactory : IImageFactory
    {
        private readonly IImageRepository _imageRepository;

        public ImageFactory(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public Stream GetImage(int height = 300, int width = 400, string query = "water")
        {
            var bytes = _imageRepository.GetImageData(query);
            var bitmap = Fill(height, width, bytes);

            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            return stream;
        }

        private Bitmap Fill(int height, int width, byte[] bytes)
        {
            return new Bitmap(new Bitmap(new MemoryStream(bytes)), width, height);
        }

        private Bitmap Resize(int height, int width, byte[] bytes)
        {
            var bitmap = new Bitmap(new MemoryStream(bytes));
            var maxDimension = Math.Max(height, width);

            //if (bitmap.Height > maxDimension && bitmap.Width > maxDimension)
            {
                bitmap = new Bitmap(bitmap, CalculateNewDimensions(bitmap, maxDimension));
            }
            return bitmap;
        }

        private static Size CalculateNewDimensions(Image originalImage, int maxDimension)
        {
            var isPortrait = (originalImage.Height > originalImage.Width);

            var aspectRatio = (isPortrait)
                                  ? Decimal.Divide(originalImage.Width, originalImage.Height)
                                  : Decimal.Divide(originalImage.Height, originalImage.Width);

            var otherDimension = Convert.ToInt32(Math.Floor(maxDimension * aspectRatio));

            return isPortrait ? new Size(otherDimension, maxDimension) : new Size(maxDimension, otherDimension);
        }
    }
}