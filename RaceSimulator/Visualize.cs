using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Controller;
using Model;
using static Model.Section;

namespace RaceSimulator
{
    public static class Visualize
    {
        public enum Rotation
        {
            North,
            East,
            South,
            West
        };

        #region definetrack
        public static Rotation NextStartDirection = Rotation.West;
        public static Rotation SectionEndDirection = Rotation.East;
        public static int Left = 0;
        public static int Top = 0;
        public static int SectionSize = 7;
        public static int CalcNewTop = 0;
        public static int CalcNewLeft = 0;
        public static int Boven = 0;
        public static int Rechts = 0;
        public static int Onder = 0;
        public static int Links = 1;
        public static int SectionCounter = 0;
        #endregion

        #region graphics

        public static string[] StraightVertical = {
            "|     |",
            "| 1   |",
            "|     |",
            "|     |",
            "|     |",
            "|   2 |",
            "|     |"
        };

        public static string[] StraightHorizontal = {
            "-------",
            "       ",
            "    1  ",
            "       ",
            " 2     ",
            "       ",
            "-------"
        };

        public static string[] LeftCornerStartWest =
        {
            "/     |",
            " 1    |" ,
            "      |" ,
            "      |" ,
            "    2 |" ,
            "      |" ,
            "------/"
        };

        public static string[] LeftCornerStartEast =
        {
            "/------",
            "|      ",
            "| 1    ",
            "|      ",
            "|   2  ",
            "|      ",
            "|     /"
        };

        public static string[] LeftCornerStartNorth =
        {
            "|     \\" ,
            "|   2  " ,
            "|      " ,
            "| 1    " ,
            "|      " ,
            "|      " ,
           "\\------"
        };

        public static string[] LeftCornerStartSouth =
        {
            "------\\",
            "      |",
            "   1  |",
            "      |",
            "  2   |",
            "      |",
           "\\     |",
        };


        public static string[] RightCornerStartNorth =
        {
            "/     |" ,
            "      |" ,
            "  2   |" ,
            "      |" ,
            "   1  |" ,
            "      |" ,
            "------/"
        };

        public static string[] RightCornerStartWest =
        {
            "------\\",
            "      |",
            "   1  |",
            "      |",
            " 2    |",
            "      |",
           "\\     |",
        };

        public static string[] RightCornerStartSouth =
        {
            "/------",
            "|      ",
            "| 1    ",
            "|      ",
            "|   2  ",
            "|      ",
            "|     //"
        };

        public static string[] RightCornerStartEast =
        {
            "|     \\",
            "|  2   " ,
            "|      " ,
            "|      " ,
            "| 1    " ,
            "|      " ,
           "\\------"
        };


        public static string[] FinishHorizontal =
        {
            "-------" ,
            "      F" ,
            "    1 F" ,
            "      F" ,
            " 2    F" ,
            "      F" ,
            "-------"
        };

        public static string[] FinishVertical =
        {
            "|     |",
            "| 1   |",
            "|     |",
            "|FFFFF|",
            "|     |",
            "|   2 |",
            "|     |"
        };

        public static string[] StartVertical =
        {
            "|     |",
            "|     |",
            "|     |",
            "|S1S2S|",
            "|     |",
            "|     |",
            "|     |"
        };

        public static string[] StartHorizontal =
        {
            "-------" ,
            "   S   " ,
            "   1   " ,
            "   S   " ,
            "   2   " ,
            "   S   " ,
            "-------"
        };

        #endregion
        public static void PrintSectionType(string[] SectionDrawing, SectionTypes section) 
        {
            string[] CopySectionDrawing = new string[SectionDrawing.Length];
            SectionDrawing.CopyTo(CopySectionDrawing, 0);
            var currentTop = Top;
            string Replace1 = " ";
            string Replace2 = " ";
            foreach (Driver racer in Data.Competition.Drivers)
            {
                if (racer.PosOnTrack == SectionCounter)
                {
                    if (racer.LeftOrRight == 0)
                    {
                        if (racer.Finish)
                        {
                        }
                        else
                        {
                            if (racer.Equipment.isBroken)
                            {
                                Replace1 = "!";
                            }
                            else
                            {
                                Replace1 = racer.Name[0].ToString();

                            }
                        }
                    }
                    else if (racer.LeftOrRight == 1)
                    {
                        if (racer.Finish)
                        {
                        }
                        else
                        {
                            if (racer.Equipment.isBroken)
                            {
                                Replace2 = "!";
                            }
                            else
                            {
                                Replace2 = racer.Name[0].ToString();
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < CopySectionDrawing.Length; i++)
            {
                CopySectionDrawing[i] = CopySectionDrawing[i].Replace("1", Replace1);
                CopySectionDrawing[i] = CopySectionDrawing[i].Replace("2", Replace2);
                Console.SetCursorPosition(Left, Top);
                Top += 1;
                Console.Write(CopySectionDrawing[i]);
            }
            Top = currentTop;
        }

        public static void MoveSection()
        {
            if (SectionEndDirection == Rotation.North)
            {
                Top = Top - SectionSize;
            }
            else if (SectionEndDirection == Rotation.East)
            {
                Left = Left + SectionSize;
            }
            else if (SectionEndDirection == Rotation.South)
            {
                Top = Top + SectionSize;
            }
            else if (SectionEndDirection == Rotation.West)
            {
                Left = Left - SectionSize;
            }
        }


        public static void DrawSectionsOnConsole(SectionTypes sectionType, Rotation startDirection)
        {
            #region Straight
            if (sectionType == SectionTypes.Straight)
            {
                if (startDirection == Rotation.East)
                {
                    PrintSectionType(StraightHorizontal, SectionTypes.Straight);
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.West)
                {

                    PrintSectionType(StraightHorizontal, SectionTypes.Straight);
                    SectionEndDirection = Rotation.East;
                    MoveSection();
                }
            }
            if (sectionType == SectionTypes.Straight)
            {
                if (startDirection == Rotation.North)
                {

                    PrintSectionType(StraightVertical, SectionTypes.Straight);
                    SectionEndDirection = Rotation.South;
                    MoveSection();
                }
                else if (startDirection == Rotation.South)
                {

                    PrintSectionType(StraightVertical, SectionTypes.Straight);
                    SectionEndDirection = Rotation.North;
                    MoveSection();
                }
            }
            #endregion
            #region LeftCorner
            else if (sectionType == SectionTypes.LeftCorner)
            {
                if (startDirection == Rotation.North)
                {

                    PrintSectionType(LeftCornerStartNorth, SectionTypes.LeftCorner);
                    NextStartDirection = Rotation.West;
                    SectionEndDirection = Rotation.East;
                    MoveSection();
                }
                else if (startDirection == Rotation.East)
                {

                    PrintSectionType(LeftCornerStartEast, SectionTypes.LeftCorner);
                    NextStartDirection = Rotation.North;
                    SectionEndDirection = Rotation.South;
                    MoveSection();
                }
                else if (startDirection == Rotation.South)
                {

                    PrintSectionType(LeftCornerStartSouth, SectionTypes.LeftCorner);
                    NextStartDirection = Rotation.East;
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.West)
                {

                    PrintSectionType(LeftCornerStartWest, SectionTypes.LeftCorner);
                    NextStartDirection = Rotation.South;
                    SectionEndDirection = Rotation.North;
                    MoveSection();
                }
            }
            #endregion
            #region RightCorner
            else if (sectionType == SectionTypes.RightCorner)
            {
                if (startDirection == Rotation.North)
                {

                    PrintSectionType(RightCornerStartNorth, SectionTypes.RightCorner);
                    NextStartDirection = Rotation.East;
                    SectionEndDirection = Rotation.West;
                    MoveSection();

                }
                else if (startDirection == Rotation.East)
                {

                    PrintSectionType(RightCornerStartEast, SectionTypes.RightCorner);
                    NextStartDirection = Rotation.South;
                    SectionEndDirection = Rotation.North;
                    MoveSection();

                }
                else if (startDirection == Rotation.South)
                {

                    PrintSectionType(RightCornerStartSouth, SectionTypes.RightCorner);
                    NextStartDirection = Rotation.West;
                    SectionEndDirection = Rotation.East;
                    MoveSection();

                }
                else if (startDirection == Rotation.West)
                {

                    PrintSectionType(RightCornerStartWest, SectionTypes.RightCorner);
                    NextStartDirection = Rotation.North;
                    SectionEndDirection = Rotation.South;
                    MoveSection();

                }
            }
            #endregion
            #region Finish
            else if (sectionType == SectionTypes.Finish)
            {
                if ((startDirection == Rotation.West))
                {

                    PrintSectionType(FinishHorizontal, SectionTypes.Finish);
                    SectionEndDirection = Rotation.East;
                    MoveSection();
                }
                else if (startDirection == Rotation.East)
                {

                    PrintSectionType(FinishHorizontal, SectionTypes.Finish);
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.North)
                {

                    PrintSectionType(FinishVertical, SectionTypes.Finish);
                    SectionEndDirection = Rotation.South;
                    MoveSection();
                }
                else if (startDirection == Rotation.South)
                {

                    PrintSectionType(FinishVertical, SectionTypes.Finish);
                    SectionEndDirection = Rotation.North;
                    MoveSection();
                }
            }
            #endregion
            #region StartGrid
            else if (sectionType == SectionTypes.StartGrid)
            {

                if (startDirection == Rotation.West)
                {
                    PrintSectionType(StartHorizontal, SectionTypes.StartGrid);
                    SectionEndDirection = Rotation.East;
                    MoveSection();
                }
                else if (startDirection == Rotation.East)
                {

                    PrintSectionType(StartHorizontal, SectionTypes.StartGrid);
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.North)
                {

                    PrintSectionType(StartVertical, SectionTypes.StartGrid);
                    SectionEndDirection = Rotation.South;
                    MoveSection();
                }
                else if (startDirection == Rotation.South)
                {

                    PrintSectionType(StartVertical, SectionTypes.StartGrid);
                    SectionEndDirection = Rotation.North;
                    MoveSection();
                }

            }

            SectionCounter++;

            #endregion
        }

        public static void CalculateStartDimensions(SectionTypes sectionType, Rotation startDirection)
        {
            #region Straight
            if (sectionType == SectionTypes.Straight)
            {
                if (startDirection == Rotation.East)
                {
                    Links++;
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.West)
                {
                    Rechts++;
                    SectionEndDirection = Rotation.East;
                    MoveSection();
                }
            }

            if (sectionType == SectionTypes.Straight)
            {
                if (startDirection == Rotation.North)
                {
                    Onder++;
                    SectionEndDirection = Rotation.South;
                    MoveSection();
                }
                else if (startDirection == Rotation.South)
                {
                    Boven++;
                    SectionEndDirection = Rotation.North;
                    MoveSection();
                }
            }

            #endregion

            #region LeftCorner

            else if (sectionType == SectionTypes.LeftCorner)
            {
                if (startDirection == Rotation.North)
                {
                    Rechts++;
                    NextStartDirection = Rotation.West;
                    SectionEndDirection = Rotation.East;
                    MoveSection();
                }
                else if (startDirection == Rotation.East)
                {
                    Onder++;
                    NextStartDirection = Rotation.North;
                    SectionEndDirection = Rotation.South;
                    MoveSection();
                }
                else if (startDirection == Rotation.South)
                {
                    Links++;
                    NextStartDirection = Rotation.East;
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.West)
                {
                    Boven++;
                    NextStartDirection = Rotation.South;
                    SectionEndDirection = Rotation.North;
                    MoveSection();
                }
            }

            #endregion

            #region RightCorner

            else if (sectionType == SectionTypes.RightCorner)
            {
                if (startDirection == Rotation.North)
                {
                    Links++;
                    NextStartDirection = Rotation.East;
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.East)
                {
                    Boven++;
                    NextStartDirection = Rotation.South;
                    SectionEndDirection = Rotation.North;
                    MoveSection();

                }
                else if (startDirection == Rotation.South)
                {
                    Rechts++;
                    NextStartDirection = Rotation.West;
                    SectionEndDirection = Rotation.East;
                    MoveSection();

                }
                else if (startDirection == Rotation.West)
                {
                    Onder++;
                    NextStartDirection = Rotation.North;
                    SectionEndDirection = Rotation.South;
                    MoveSection();

                }
            }

            #endregion

            #region Finish

            else if (sectionType == SectionTypes.Finish)
            {
                if ((startDirection == Rotation.West))
                {
                    Links++;
                    SectionEndDirection = Rotation.East;
                    MoveSection();
                }
                else if (startDirection == Rotation.East)
                {
                    Rechts++;
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.North)
                {
                    Boven++;
                    SectionEndDirection = Rotation.South;
                    MoveSection();
                }
                else if (startDirection == Rotation.South)
                {
                    Onder++;
                    SectionEndDirection = Rotation.North;
                    MoveSection();
                }
            }

            #endregion

            #region StartGrid

            else if (sectionType == SectionTypes.StartGrid)
            {
                if (startDirection == Rotation.West)
                {
                    Rechts++;
                    SectionEndDirection = Rotation.East;
                    MoveSection();
                }
                else if (startDirection == Rotation.East)
                {
                    Links++;
                    SectionEndDirection = Rotation.West;
                    MoveSection();
                }
                else if (startDirection == Rotation.North)
                {
                    Onder++;
                    SectionEndDirection = Rotation.South;
                    MoveSection();
                }
                else if (startDirection == Rotation.South)
                {
                    Boven++;
                    SectionEndDirection = Rotation.North;
                    MoveSection();
                }

            }
            #endregion

            if (Boven > Onder)
            {
                CalcNewTop = (Boven - Onder) * SectionSize;
            }

            if (Links > Rechts)
            {
                CalcNewLeft = (Links - Rechts) * SectionSize;
            }
        }

        public static void DrawTrack(Track track)
        {
            CalcNewLeft = 0;
            CalcNewTop = 0;
            Left = 0;
            Top = 0;
            Boven = 0;
            Onder = 0;
            Links = 1;
            Rechts = 0;
            foreach (var section in track.Sections)
            {
                CalculateStartDimensions(section.SectionType, NextStartDirection);
            }

            Left = CalcNewLeft;
            Top = CalcNewTop;
            NextStartDirection = Rotation.West;
            SectionEndDirection = Rotation.East;
            SectionCounter = 0;

            foreach (var section in track.Sections)
            {
                DrawSectionsOnConsole(section.SectionType, NextStartDirection);
            }

        }

        public static void DriversChangedHandler(Object sender, DriversChangedEventArgs args)
        {
            Console.CursorVisible = false;
            DrawTrack(args.Track);
        }

        //public static void MainLoop()
        //{
        //    while (true)
        //    {
        //        Console.CursorVisible = false;
        //        Thread.Sleep(100);
        //        if (Data.CurrentRace.SomethingChanged)
        //        {
        //            // moet hier zorgen dat er een event wordt afgevuurd dat de drivers veranderd zijn.
        //            Data.CurrentRace.SomethingChanged = false;
        //            DriversChangedHandler("DriverPosChanged", new DriversChangedEventArgs(Data.CurrentRace.Track)); // volgens mij is dit nu event?
        //        }
        //    }
        //}
    }
}
