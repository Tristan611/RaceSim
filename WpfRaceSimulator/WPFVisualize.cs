using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using Model;
using static WpfRaceSimulator.CreatePicture;

namespace WpfRaceSimulator
{
    public static class WPFVisualize
    {
        public enum Direction
        {
            North,
            East,
            South,
            West
        }

        private static Bitmap RotateBitmap(Bitmap bmp, Direction dir)
        {
            Bitmap rotated = (Bitmap)bmp.Clone();

            switch (dir)
            {
                case Direction.North:
                    rotated.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Direction.East:
                    break;
                case Direction.South:
                    rotated.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Direction.West:
                    rotated.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
            }

            return rotated;
        }

        public class SectionDrawInfo
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Direction Direction { get; set; }
        }

        public static List<SectionDrawInfo> LastDrawnSections { get; private set; } = new();

        private class TrackPosition
        {
            public Section Section { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public Direction Direction { get; set; }
        }

        public static BitmapSource DrawTrack(Track track)
        {
            LastDrawnSections.Clear();

            int sectionSize = 64;
            int margin = 2;

            var positions = CalculateTrackPositions(track);

            int minX = positions.Min(p => p.X);
            int maxX = positions.Max(p => p.X);
            int minY = positions.Min(p => p.Y);
            int maxY = positions.Max(p => p.Y);

            int trackWidthSections = maxX - minX + 1 + (margin * 2);
            int trackHeightSections = maxY - minY + 1 + (margin * 2);

            int width = trackWidthSections * sectionSize;
            int height = trackHeightSections * sectionSize;

            Bitmap bmp = GiveEmptyBitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (var position in positions)
                {
                    int drawX = position.X - minX + margin;
                    int drawY = position.Y - minY + margin;

                    string imageName = GetSectionImageName(position.Section.SectionType, position.Direction);
                    Bitmap sectionBmp = AddToPictureCache(imageName);

                    LastDrawnSections.Add(new SectionDrawInfo
                    {
                        X = drawX,
                        Y = drawY,
                        Direction = position.Direction
                    });

                    g.DrawImage(
                        sectionBmp,
                        drawX * sectionSize,
                        drawY * sectionSize,
                        sectionSize,
                        sectionSize
                    );
                }
            }

            return CreateBitmapSourceFromGdiBitmap(bmp);
        }

        private static string GetSectionImageName(Section.SectionTypes type, Direction dir)
        {
            return type switch
            {
                Section.SectionTypes.Finish => dir is Direction.North or Direction.South
                    ? "FinishRaceSim.png"
                    : "FinishRaceSimVertical.png",
                Section.SectionTypes.RightCorner => dir switch
                {
                    Direction.North => "LeftCornerStartNorthRaceSim.png",
                    Direction.East => "LeftCornerStartEastRaceSim.png",
                    Direction.South => "LeftCornerStartSouthRaceSim.png",
                    Direction.West => "LeftCornerStartWestRaceSim.png",
                    _ => "LeftCornerStartEastRaceSim.png"
                },
                Section.SectionTypes.LeftCorner => dir switch
                {
                    Direction.North => "RightCornerStartNorthRaceSim.png",
                    Direction.East => "RightCornerStartEastRaceSim.png",
                    Direction.South => "RightCornerStartSouthRaceSim.png",
                    Direction.West => "RightCornerStartWestRaceSim.png",
                    _ => "RightCornerStartEastRaceSim.png"
                },
                Section.SectionTypes.Straight => dir is Direction.North or Direction.South
                    ? "StraightRaceSim.png"
                    : "StraightHorizontalRaceSim.png",
                Section.SectionTypes.StartGrid => dir is Direction.North or Direction.South
                    ? "StartGridVertical.png"
                    : "StartGridHorizontal.png",
                _ => "StraightHorizontalRaceSim.png"
            };
        }

        private static List<TrackPosition> CalculateTrackPositions(Track track)
        {
            var positions = new List<TrackPosition>();

            int x = 0;
            int y = 0;
            Direction dir = Direction.East;

            foreach (var section in track.Sections)
            {
                Direction prevDir = dir;

                if (section.SectionType == Section.SectionTypes.LeftCorner)
                {
                    dir = dir switch
                    {
                        Direction.North => Direction.West,
                        Direction.West => Direction.South,
                        Direction.South => Direction.East,
                        Direction.East => Direction.North,
                        _ => dir
                    };
                }
                else if (section.SectionType == Section.SectionTypes.RightCorner)
                {
                    dir = dir switch
                    {
                        Direction.North => Direction.East,
                        Direction.East => Direction.South,
                        Direction.South => Direction.West,
                        Direction.West => Direction.North,
                        _ => dir
                    };
                }

                Direction drawDir = section.SectionType == Section.SectionTypes.Finish ? prevDir : dir;

                positions.Add(new TrackPosition
                {
                    Section = section,
                    X = x,
                    Y = y,
                    Direction = drawDir
                });

                switch (dir)
                {
                    case Direction.North: y--; break;
                    case Direction.South: y++; break;
                    case Direction.East: x++; break;
                    case Direction.West: x--; break;
                }
            }

            return positions;
        }
    }
}