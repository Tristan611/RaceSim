using Controller;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WpfRaceSimulator.ViewModels
{
    public class RaceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string CurrentTrackName => Data.CurrentRace?.Track.Name ?? "Geen race actief";

        public ObservableCollection<Driver> Drivers { get; set; } = new();

        public ObservableCollection<Driver> ChampionshipStandings { get; set; } = new();

        public string BestOnPoints => Data.Competition?.PointsResults.GetBestParticipant() ?? "";
        public string FastestFinisher => Data.Competition?.TimeResults.GetBestParticipant() ?? "";

        public RaceViewModel()
        {
            Refresh();

            Data.RaceChanged += (s, e) => Refresh();

            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += (s, e) => Refresh();
            }
        }

        public void Refresh()
        {
            Drivers.Clear();
            ChampionshipStandings.Clear();

            if (Data.Competition != null)
            {
                foreach (var driver in Data.Competition.Drivers)
                {
                    Drivers.Add(driver);
                }

                foreach (var driver in Data.Competition.Drivers.OrderByDescending(d => d.Points))
                {
                    ChampionshipStandings.Add(driver);
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}