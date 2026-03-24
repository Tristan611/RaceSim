using System.Diagnostics;
using System.Timers;
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
        private static System.Timers.Timer _timer;
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
            int PositionOnTrack = 0;
            int LeftOrRight = 0;
            if (drivers.Count % 2 != 0)
            {
                LeftOrRight = 1;
            }

            foreach (Driver racer in Drivers)
            {
                racer.LeftOrRight = LeftOrRight;
                racer.PosOnTrack = PositionOnTrack;
                racer.Lap = 0;
                if (LeftOrRight == 0)
                {
                    LeftOrRight = 1;
                }
                else
                {
                    LeftOrRight = 0;
                    PositionOnTrack++;
                }
            }
        }
        public Race(Track track, List<Driver> drivers)
        {
            this.Track = track;
            this.Drivers = drivers;
            RandomizeEquipment();
            _timer = new System.Timers.Timer(500);
            _timer.Elapsed += OnTimedEvent;
            StartTimer();
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
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            foreach (var driver in Drivers)
            {
                Thread.Sleep(1);
                SomethingChanged = false;
                if (driver.Equipment.isBroken == false)
                {
                    BananenSchil(driver);
                    driver.CheckNextposition();
                    if (driver.Finish)
                    {
                        SomethingChanged = false;
                        if (FinishedDrivers.ContainsKey(driver))
                        {
                        }
                        else
                        {
                            FinishedDrivers.Add(driver, +1);
                        }
                    }
                    if (driver.ChangedDriverPosition)
                    {
                        Debug.WriteLine("ik zou moeten invoken");
                        //SomethingChanged = true;
                        DriversChanged?.Invoke(this, new DriversChangedEventArgs(Data.CurrentRace.Track));
                    }
                }
                else
                {
                    MagWeerVerderRijden(driver);
                }

            }



            if (FinishedDrivers.Count == Drivers.Count)
            {
                FinishedDrivers.Clear();
                SomethingChanged = false;
                _timer.Stop();                              // Kan nog een klein insectje veroorzaken.
                Console.Clear();
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
                if (driver.Equipment.Speed == 4)
                {
                    driver.Equipment.Speed = 4;
                }
                else
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

        public void StartTimer()
        {
            _timer.Start();
        }
    }
}
