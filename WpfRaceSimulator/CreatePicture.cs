using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Model;
using Color = System.Drawing.Color;

namespace WpfRaceSimulator
{
    public static class CreatePicture
    {
        private static Dictionary<string, Bitmap> PictureClassCache = new Dictionary<string, Bitmap>();
        
        public static Bitmap AddToPictureCache(string url)
        {
            Bitmap bitmap = null;
            if (!PictureClassCache.ContainsKey(url))
            { 
                PictureClassCache.Add(url, new Bitmap(url));
                PictureClassCache.TryGetValue(url, out bitmap);
            }
            else
            {
                PictureClassCache.TryGetValue(url, out bitmap);
            }
            return bitmap;
        }

        public static void ClearCache()
        {
            PictureClassCache.Clear();
        }

        public static Bitmap GiveEmptyBitmap(int x, int y)
        {
            string empty = "empty";
            Bitmap emptyBitMap = null;
            if (PictureClassCache.ContainsKey(empty))
            {
                emptyBitMap = PictureClassCache[empty];
            }
            else
            {
                emptyBitMap = new Bitmap("empty");
                emptyBitMap.SetResolution(x,y);
                SolidBrush kleur = new SolidBrush(Color.Lime);
                Graphics bitGraphics = Graphics.FromImage(emptyBitMap);
                bitGraphics.FillRectangle(kleur,0,0,x,y);
                PictureClassCache.Add(empty, emptyBitMap);
            }

            Bitmap newEmptyBitmap = null;
            newEmptyBitmap = (Bitmap)emptyBitMap.Clone();
            return newEmptyBitmap;
        }
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        public static void CalculateImageDimensions()
        {
           
        }
    }

}
