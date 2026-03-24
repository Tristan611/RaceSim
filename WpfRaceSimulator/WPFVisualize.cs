using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Model;
using static WpfRaceSimulator.CreatePicture;

namespace WpfRaceSimulator
{
    public static class WPFVisualize
    {
        public static BitmapSource DrawTrack(Track track)
        {
            Bitmap empty = GiveEmptyBitmap(64, 64);
            BitmapSource bmp = CreateBitmapSourceFromGdiBitmap(empty);
            return bmp;
        }
    }
}
