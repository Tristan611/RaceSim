using System.Diagnostics;
using Model;
using static Model.DriversChangedEventArgs;

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
        public Dictionary<Driver, int> FinishedDrivers = new Dictionary<Driver, int>();
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
                        if (!FinishedDrivers.ContainsKey(driver))
                        {
                            FinishedDrivers.Add(driver, 1);
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
                FinishedDrivers.Clear();

                foreach (var driver in Drivers)
                {
                    driver.ResetDrivers();
                }

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

        public void MagWeerVerderRijden(Driver driver)
        {
            if (_random2.Next(0, 50) > 35)
            {
                driver.Equipment.isBroken = false;
            }
        }
    }
}