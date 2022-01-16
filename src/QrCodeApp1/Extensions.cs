using System.IO;
using SkiaSharp;

namespace Net.Codecrete.QrCodeGenerator
{
    public static class Extensions
    {
        public static SKBitmap ToBitmap(this QrCode qrCode, int scale, int border, SKColor foreground, SKColor background)
        {
            // check arguments
            if (scale <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scale), "Value out of range");
            }
            if (border < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(border), "Value out of range");
            }

            int size = qrCode.Size;
            int dim = (size + border * 2) * scale;

            if (dim > short.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(scale), "Scale or border too large");
            }

            // create bitmap
            SKBitmap bitmap = new SKBitmap(dim, dim, SKColorType.Rgb888x, SKAlphaType.Opaque);

            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // draw background
                using (SKPaint paint = new SKPaint { Color = background })
                {
                    canvas.DrawRect(0, 0, dim, dim, paint);
                }

                // draw modules
                using (SKPaint paint = new SKPaint { Color = foreground })
                {
                    for (int y = 0; y < size; y++)
                    {
                        for (int x = 0; x < size; x++)
                        {
                            if (qrCode.GetModule(x, y))
                            {
                                canvas.DrawRect((x + border) * scale, (y + border) * scale, scale, scale, paint);
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        /// Creates a bitmap of QR code.

        public static SKBitmap ToBitmap(this QrCode qrCode, int scale, int border)
        {
            return qrCode.ToBitmap(scale, border, SKColors.Black, SKColors.White);
        }

        public static byte[] ToPng(this QrCode qrCode, int scale, int border, SKColor foreground, SKColor background)
        {
            using SKBitmap bitmap = qrCode.ToBitmap(scale, border, foreground, background);
            using SKData data = bitmap.Encode(SKEncodedImageFormat.Png, 90);
            return data.ToArray();
        }

        /// Creates a PNG image of QR code and returns it as a byte array.
        public static byte[] ToPng(this QrCode qrCode, int scale, int border)
        {
            return qrCode.ToPng(scale, border, SKColors.Black, SKColors.White);
        }

        public static void SaveAsPng(this QrCode qrCode, string filename, int scale, int border, SKColor foreground, SKColor background)
        {
            using SKBitmap bitmap = qrCode.ToBitmap(scale, border, foreground, background);
            using SKData data = bitmap.Encode(SKEncodedImageFormat.Png, 90);
            using FileStream stream = File.OpenWrite(filename);
            data.SaveTo(stream);
        }

        /// Saves QR code as a PNG file.
        public static void SaveAsPng(this QrCode qrCode, string filename, int scale, int border)
        {
            qrCode.SaveAsPng(filename, scale, border, SKColors.Black, SKColors.White);
        }
    }
}
