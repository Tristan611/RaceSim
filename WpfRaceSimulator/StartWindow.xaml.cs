using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfRaceSimulator
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedDrivers = new List<string>();

            if (MarioCheckBox.IsChecked == true) selectedDrivers.Add("Mario");
            if (LuigiCheckBox.IsChecked == true) selectedDrivers.Add("Luigi");
            if (YoshiCheckBox.IsChecked == true) selectedDrivers.Add("Yoshi");
            if (PeachCheckBox.IsChecked == true) selectedDrivers.Add("Peach");
            if (BowserCheckBox.IsChecked == true) selectedDrivers.Add("Bowser");

            List<string> selectedTracks = new List<string>();

            if (WideOvalCheckBox.IsChecked == true) selectedTracks.Add("Wide Oval");
            if (ChicaneLoopCheckBox.IsChecked == true) selectedTracks.Add("Chicane Loop");
            if (DoubleBendCheckBox.IsChecked == true) selectedTracks.Add("Double Bend");
            if (TechnicalLoopCheckBox.IsChecked == true) selectedTracks.Add("Technical Loop");
            if (LongSnakeCheckBox.IsChecked == true) selectedTracks.Add("Long Snake");

            if (selectedDrivers.Count < 3)
            {
                MessageBox.Show("Selecteer minimaal 3 racers.");
                return;
            }

            if (selectedTracks.Count < 2)
            {
                MessageBox.Show("Selecteer minimaal 2 circuits.");
                return;
            }

            MainWindow mainWindow = new MainWindow(selectedDrivers, selectedTracks);
            mainWindow.Show();

            Close();
        }
    }
}
