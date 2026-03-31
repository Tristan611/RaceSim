using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace WpfRaceSimulator
{
    public static class CreatePicture
    {
        private static Dictionary<string, Bitmap> PictureClassCache = new Dictionary<string, Bitmap>();

        /// <summary>
        /// Haal een bitmap uit de cache of laad hem vanaf schijf.
        /// </summary>
        public static Bitmap AddToPictureCache(string filename)
        {
            Bitmap bitmap = null;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pictures", filename);

            if (!PictureClassCache.TryGetValue(path, out bitmap))
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Afbeelding niet gevonden: {path}");

                // Probeer toe te voegen, maar vang de exception af als het toch al bestaat
                try
                {
                    PictureClassCache.Add(path, new Bitmap(path));
                }
                catch (ArgumentException)
                {
                    // Iemand anders heeft hem net toegevoegd, dat is oké
                }
                PictureClassCache.TryGetValue(path, out bitmap);
            }
            return bitmap;
        }

        /// <summary>
        /// Leeg de cache (optioneel te gebruiken bij trackwissel).
        /// </summary>
        public static void ClearCache()
        {
            foreach (var bmp in PictureClassCache.Values)
            {
                bmp.Dispose();
            }
            PictureClassCache.Clear();
        }

        /// <summary>
        /// Geef een lege bitmap van de gewenste grootte.
        /// </summary>
        public static Bitmap GiveEmptyBitmap(int width, int height)
        {
            Bitmap emptyBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(emptyBitmap))
            {
                g.Clear(Color.Lime); // Of een andere achtergrondkleur
            }
            return emptyBitmap;
        }

        /// <summary>
        /// Zet een GDI+ Bitmap om naar een WPF BitmapSource.
        /// </summary>
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    System.Windows.Media.PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    bitmapData.Stride * bitmapData.Height,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}