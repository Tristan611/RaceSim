using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Controller;
using Model;


namespace WpfRaceSimulator
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private Dictionary<Driver, Image> driverElements = new Dictionary<Driver, Image>();
        private const int SectionSize = 64;
        private const int DriverSize = 20;

        public MainWindow()
        {
            InitializeComponent();

            Data.Initialize();

            InitializeDriverUI();

            Data.CurrentRace.DriversChanged += DriversChangedHandler;
            Data.RaceChanged += OnRaceChanged;

            TrackImage.Source = WPFVisualize.DrawTrack(Data.CurrentRace.Track);
            UpdateDriverPositions();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += (s, e) =>
            {
                Data.CurrentRace.Update();
            };
            _timer.Start();
        }

        private void OnRaceChanged(object sender, EventArgs e)
        {
            Data.CurrentRace.DriversChanged += DriversChangedHandler;

            TrackImage.Source = WPFVisualize.DrawTrack(Data.CurrentRace.Track);
            UpdateDriverPositions();
        }

        private void InitializeDriverUI()
        {
            foreach (var driver in Data.CurrentRace.Drivers)
            {
                Image driverImage = new Image
                {
                    Width = DriverSize,
                    Height = DriverSize,
                    Stretch = Stretch.Uniform
                };

                driverImage.Source = GetDriverImage(driver);

                MainCanvas.Children.Add(driverImage);
                driverElements[driver] = driverImage;
            }
        }

        private BitmapImage GetDriverImage(Driver driver)
        {
            string fileName = driver.Name switch
            {
                "Mario" => "MarioKartRaceSim.png",
                "Luigi" => "LuigiKartRaceSim.png",
                "Wario" => "YoshiKartRaceSim.png",
                "Peach" => "YoshiKartRaceSim.png",
                "Bowser" => "MarioKartRaceSim.png",
                _ => "MarioKartRaceSim.png"
            };

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pictures", fileName);

            return new BitmapImage(new Uri(path, UriKind.Absolute));
        }

        private Brush GetBrush(IParticipant.TeamColors color)
        {
            return color switch
            {
                IParticipant.TeamColors.Red => Brushes.Red,
                IParticipant.TeamColors.Green => Brushes.Green,
                IParticipant.TeamColors.Yellow => Brushes.Yellow,
                IParticipant.TeamColors.Grey => Brushes.Gray,
                IParticipant.TeamColors.Blue => Brushes.Blue,
                _ => Brushes.Black
            };
        }

        public void DriversChangedHandler(object sender, DriversChangedEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                TrackImage.Source = WPFVisualize.DrawTrack(args.Track);
                UpdateDriverPositions();
            });
        }

        private void UpdateDriverPositions()
        {
            foreach (var driver in Data.CurrentRace.Drivers)
            {
                if (!driverElements.ContainsKey(driver))
                    continue;

                if (driver.PosOnTrack < 0 || driver.PosOnTrack >= WPFVisualize.LastDrawnSections.Count)
                    continue;

                var drawInfo = WPFVisualize.LastDrawnSections[driver.PosOnTrack];
                var driverImage = driverElements[driver];

                driverImage.RenderTransformOrigin = new Point(0.5, 0.5);

                Transform transform = drawInfo.Direction switch
                {
                    // → normaal
                    WPFVisualize.Direction.East => new RotateTransform(0),

                    // ↓ draaien
                    WPFVisualize.Direction.South => new RotateTransform(90),

                    // ↑ draaien
                    WPFVisualize.Direction.North => new RotateTransform(270),

                    // ← spiegelen (NIET roteren)
                    WPFVisualize.Direction.West => new ScaleTransform(-1, 1),

                    _ => new RotateTransform(0)
                };

                driverImage.RenderTransform = transform;

                double baseX = drawInfo.X * SectionSize;
                double baseY = drawInfo.Y * SectionSize;

                double left;
                double top;

                switch (drawInfo.Direction)
                {
                    case WPFVisualize.Direction.East:
                        left = baseX + 18;
                        top = driver.LeftOrRight == 0 ? baseY + 10 : baseY + 34;
                        break;

                    case WPFVisualize.Direction.West:
                        left = baseX + 18;
                        top = driver.LeftOrRight == 0 ? baseY + 34 : baseY + 10;
                        break;

                    case WPFVisualize.Direction.North:
                        left = driver.LeftOrRight == 0 ? baseX + 10 : baseX + 34;
                        top = baseY + 18;
                        break;

                    case WPFVisualize.Direction.South:
                        left = driver.LeftOrRight == 0 ? baseX + 34 : baseX + 10;
                        top = baseY + 18;
                        break;

                    default:
                        left = baseX + 18;
                        top = baseY + 18;
                        break;
                }

                Canvas.SetLeft(driverImage, left);
                Canvas.SetTop(driverImage, top);
            }
        }
    }
}