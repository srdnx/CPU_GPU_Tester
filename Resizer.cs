using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_GPU_Tester
{
    class Resizer
    {
        private readonly static object Locker = new object();

        public static double[][] ResizeImage(Image image, int width, int height)
        {

            Bitmap destImage;

            // Low quality resizing
            lock (Locker)
            {
                if (image.Height == height)
                {
                    destImage = new Bitmap(image);
                }
                else
                {
                    destImage = new Bitmap(image, new Size((int)(((double)height / image.Height) * image.Width), height));
                }
            }

            /*
            // High quality, but more time
            lock (Locker)
            {
                Rectangle destRect = new Rectangle(0, 0, width, height);
                destImage = new Bitmap(width, height)

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }
            }
            */

            double[][] result = new double[width][];
            for (int i = 0; i < width; i++)
            {
                result[i] = new double[height];
                for (int j = 0; j < height; j++)
                {
                    if (destImage.Width <= i)
                    {
                        result[i][j] = 0;
                    }
                    else
                    {
                        result[i][j] = inGray(destImage.GetPixel(i, j).R, destImage.GetPixel(i, j).G, destImage.GetPixel(i, j).B);
                    }
                }
            }

            return result;
        }

        public static double inGray(int R, int G, int B)
        {
            return 1.0 - (0.2126 * R + 0.7152 * G + 0.0722 * B) / 255;
        }

    }
}
