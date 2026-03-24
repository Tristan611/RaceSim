using System.Diagnostics;
using static Model.IParticipant;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors teamColors { get; set; }
        public int PosOnTrack { get; set; }
        public int Lap { get; set; }
        public int LeftOrRight { get; set; }
        public int RaceProgress { get; set; }                           // Aantel meters voortgang
        private Competition Competition { get; set; }
        public bool ChangedDriverPosition { get; set; }
        public bool Finish = false;


        public Driver(string name, int points, TeamColors teamColors, IEquipment equipment, Competition competition)
        {
            Name = name;
            Points = points;
            Equipment = equipment;
            this.teamColors = teamColors;
            Competition = competition;
        }

        public void ResetDrivers()
        {
            PosOnTrack = 0;
            Lap = 0;
            LeftOrRight = 0;
            RaceProgress = 0;
            Finish = false;
        }

        public int CalculateDriverSpeed()
        {
            return Equipment.Performance * Equipment.Speed;
        }

        public void CheckNextposition()
        {
            int TotalTrackLength = (Competition.CurrentRaceTrack.Sections.Count * 100) - 100;
            int NextRacePosition = RaceProgress + CalculateDriverSpeed();
            int ExpPosOnTrack = (NextRacePosition % TotalTrackLength) / 100; // nog even checken
            Lap = (NextRacePosition / TotalTrackLength);
            bool PosLeftAvailable = true;
            bool PosRightAvailable = true;

            foreach (var driver in Competition.Drivers)
            {
                {
                    if (driver.PosOnTrack == ExpPosOnTrack && driver.Name != Name)
                    {
                        if (driver.LeftOrRight == 0)
                        {
                            PosLeftAvailable = false;
                        }
                        else
                        {
                            PosRightAvailable = false;
                        }

                    }
                }

            }

            if (PosLeftAvailable == false && PosRightAvailable == false)
            {
                return;
            }

            if (Lap == 3)
            {
                Finish = true;
                RaceProgress += 100;
            }

            RaceProgress = NextRacePosition;

            if (ExpPosOnTrack == PosOnTrack)
            {
                return;
            }

             Debug.WriteLine("Ik zet hier driverChangedPos op true");

            ChangedDriverPosition = true;
            PosOnTrack = ExpPosOnTrack;

            if (PosLeftAvailable)
            {
                LeftOrRight = 0;
            }
            else
            {
                LeftOrRight = 1;
            }


        }


    }
}