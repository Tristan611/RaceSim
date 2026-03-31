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
            int totalTrackLength = Competition.CurrentRaceTrack.Sections.Count * 100;
            int nextRacePosition = RaceProgress + CalculateDriverSpeed();

            if (nextRacePosition >= totalTrackLength * 3)
            {
                Finish = true;
                RaceProgress = totalTrackLength * 3;
                return;
            }

            int expPosOnTrack = (nextRacePosition % totalTrackLength) / 100;
            Lap = nextRacePosition / totalTrackLength;

            bool posLeftAvailable = true;
            bool posRightAvailable = true;

            foreach (var driver in Competition.Drivers)
            {
                if (driver.PosOnTrack == expPosOnTrack && driver.Name != Name && !driver.Finish)
                {
                    if (driver.LeftOrRight == 0)
                        posLeftAvailable = false;
                    else
                        posRightAvailable = false;
                }
            }

            if (!posLeftAvailable && !posRightAvailable)
            {
                return;
            }

            RaceProgress = nextRacePosition;

            if (expPosOnTrack == PosOnTrack)
            {
                return;
            }

            ChangedDriverPosition = true;
            PosOnTrack = expPosOnTrack;
            LeftOrRight = posLeftAvailable ? 0 : 1;
        }


    }
}