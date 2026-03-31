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
            if (Data.CurrentRace == null)
            {
                _timer.Stop();
                MessageBox.Show("Alle races zijn klaar!");
                return;
            }

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
                if (driver.Finish)
                {
                    driverElements[driver].Visibility = Visibility.Hidden;
                    continue;
                }
                else
                {
                    driverElements[driver].Visibility = Visibility.Visible;
                }
                if (!driverElements.ContainsKey(driver))
                    continue;

                if (driver.PosOnTrack < 0 || driver.PosOnTrack >= WPFVisualize.LastDrawnSections.Count)
                    continue;

                var drawInfo = WPFVisualize.LastDrawnSections[driver.PosOnTrack];
                var driverImage = driverElements[driver];

                driverImage.RenderTransformOrigin = new Point(0.5, 0.5);

                var transformGroup = new TransformGroup();

                // basisrichting
                switch (drawInfo.Direction)
                {
                    case WPFVisualize.Direction.East:
                        transformGroup.Children.Add(new RotateTransform(0));
                        break;

                    case WPFVisualize.Direction.South:
                        transformGroup.Children.Add(new RotateTransform(90));
                        break;

                    case WPFVisualize.Direction.North:
                        transformGroup.Children.Add(new RotateTransform(270));
                        break;

                    case WPFVisualize.Direction.West:
                        transformGroup.Children.Add(new ScaleTransform(-1, 1));
                        break;
                }

                // extra spin als kapot
                if (driver.Equipment.isBroken)
                {
                    double spinAngle = (Environment.TickCount / 10) % 360;
                    transformGroup.Children.Add(new RotateTransform(spinAngle));
                }

                driverImage.RenderTransform = transformGroup;

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