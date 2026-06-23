using System.Diagnostics;
using Model;
using static Model.DriversChangedEventArgs;
using System.Windows;

namespace Controller
{
    public class Race
    {
        public event DriversChangedEventHandler DriversChanged;

        public Track Track { get; set; }
        public List<Driver> Drivers { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random = new Random(DateTime.Now.Millisecond);

        public bool SomethingChanged = false;
        public List<Driver> FinishedDrivers = new List<Driver>();
        public Random _random2 = new Random();

        private Dictionary<Section, SectionData> _positions { get; set; }

        public SectionData GetSectionData(Section section)
        {
            if (_positions.TryGetValue(section, out var data))
            {
                return data;
            }
            else
            {
                _positions.Add(section, new SectionData());
                return _positions.Last().Value;
            }
        }

        public void SetPlayerStartPositions(Track track, List<Driver> drivers)
        {
            _positions = new Dictionary<Section, SectionData>();

            int positionOnTrack = 0;
            int leftOrRight = 0;

            if (drivers.Count % 2 != 0)
            {
                leftOrRight = 1;
            }

            foreach (Driver racer in Drivers)
            {
                racer.LeftOrRight = leftOrRight;
                racer.PosOnTrack = positionOnTrack;
                racer.Lap = 0;
                racer.RaceProgress = 0;
                racer.Finish = false;
                racer.ChangedDriverPosition = false;

                if (leftOrRight == 0)
                {
                    leftOrRight = 1;
                }
                else
                {
                    leftOrRight = 0;
                    positionOnTrack++;
                }
            }
        }

        public Race(Track track, List<Driver> drivers)
        {
            Track = track;
            Drivers = drivers;
            StartTime = DateTime.Now;

            RandomizeEquipment();
            SetPlayerStartPositions(track, drivers);
        }

        public void RandomizeEquipment()
        {
            foreach (var driver in Drivers)
            {
                if (driver.Equipment.Quality == 0)
                {
                    driver.Equipment.Quality = _random.Next(75, 100);
                    driver.Equipment.Performance = _random.Next(8, 14);
                    driver.Equipment.Speed = _random.Next(8, 15);
                }
            }
        }

        public void Update()
        {
            bool changed = false;

            foreach (var driver in Drivers)
            {
                driver.ChangedDriverPosition = false;

                if (!driver.Equipment.isBroken)
                {
                    BananenSchil(driver);
                    driver.CheckNextposition();

                    if (driver.Finish)
                    {
                        if (!FinishedDrivers.Contains(driver))
                        {
                            FinishedDrivers.Add(driver);
                            changed = true;
                        }
                    }

                    if (driver.ChangedDriverPosition)
                    {
                        changed = true;
                    }
                }
                else
                {
                    MagWeerVerderRijden(driver);
                }
            }

            if (changed)
            {
                DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));
            }

            if (FinishedDrivers.Count == Drivers.Count)
            {
                GivePoints();

                string result = $"Eindstand van {Track.Name}\n\n";

                for (int i = 0; i < FinishedDrivers.Count; i++)
                {
                    result += $"{i + 1}. {FinishedDrivers[i].Name} - {FinishedDrivers[i].Points} punten totaal\n";
                }

                result += "\nTussenstand kampioenschap:\n";

                var championshipStandings = Drivers
                    .OrderByDescending(driver => driver.Points)
                    .ToList();

                for (int i = 0; i < championshipStandings.Count; i++)
                {
                    result += $"{i + 1}. {championshipStandings[i].Name} - {championshipStandings[i].Points} punten\n";
                }

                result += "\nStatistieken:\n";
                result += $"Beste op punten: {Data.Competition.PointsResults.GetBestParticipant()}\n";
                result += $"Snelste finisher: {Data.Competition.TimeResults.GetBestParticipant()}\n";

                Data.RaceFinished?.Invoke(result);
                    
                FinishedDrivers.Clear();
                Data.NextRace();
            }
        }

        public void BananenSchil(Driver driver)
        {
            if (_random2.Next(0, 50) == 5)
            {
                driver.Equipment.isBroken = true;

                if (driver.Equipment.Speed > 4)
                {
                    driver.Equipment.Speed -= 1;
                }
            }
        }

        public void GivePoints()
        {
            int[] pointsPerPosition = { 10, 8, 6, 4, 2 };

            for (int i = 0; i < FinishedDrivers.Count; i++)
            {
                if (i < pointsPerPosition.Length)
                {
                    int points = pointsPerPosition[i];

                    FinishedDrivers[i].Points += points;

                    Data.Competition.PointsResults.Add(
                        new PointsResult(FinishedDrivers[i].Name, points)
                    );

                    Data.Competition.TimeResults.Add(
                        new TimeResult(FinishedDrivers[i].Name, DateTime.Now - StartTime)
                    );
                }
            }
        }

        public void MagWeerVerderRijden(Driver driver)
        {
            if (_random2.Next(0, 50) > 35)
            {
                driver.Equipment.isBroken = false;
            }
        }
    }
}