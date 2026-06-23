using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Model;


namespace Controller
{
    public static class Data
    {
        public static event EventHandler RaceChanged;

        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static Action<string> RaceFinished;

        public static Action<string> ChampionshipFinished;
        public static void Initialize ()
       {
           Competition = new Competition();
           AddTracks();
           AddParticipants();
           NextRace();
       }

       public static void AddParticipants()
       {
           Competition.Drivers.Add(new Driver("Mario", 0, IParticipant.TeamColors.Red, new Car(0, 10, 10, false), Competition));
           Competition.Drivers.Add(new Driver("Luigi", 0, IParticipant.TeamColors.Green, new Car(0, 7, 10, false), Competition));
           Competition.Drivers.Add(new Driver("Wario", 0, IParticipant.TeamColors.Yellow, new Car(0, 8, 10, false), Competition));
           Competition.Drivers.Add(new Driver("Peach", 0, IParticipant.TeamColors.Blue, new Car(0, 9, 10, false), Competition));
           Competition.Drivers.Add(new Driver("Bowser", 0, IParticipant.TeamColors.Grey, new Car(0, 11, 10, false), Competition));

        }

        public static void AddTracks()
        {
            var S = Section.SectionTypes.Straight;
            var L = Section.SectionTypes.LeftCorner;
            var R = Section.SectionTypes.RightCorner;
            var G = Section.SectionTypes.StartGrid;
            var F = Section.SectionTypes.Finish;

            Section.SectionTypes[] track1_WideOval =
            {
                G, G,
                S, S, S, S, S,
                R,
                S, S, S,
                R,
                S, S, S, S, S, S, S, S,
                R,
                S, S, S,
                R,
                F
            };

            Section.SectionTypes[] track2_ChicaneLoop =
            {
                G, G,
                S, R, L, L, R, S, S,
                R,
                S, S,
                R,
                S, S, R, L, L, R, S, S,
                R,
                S, S,
                R,
                F
            };

            Section.SectionTypes[] track3_DoubleBend =
            {
                G, G,
                S, S, R, L, L, R, S,
                R,
                S, S, S,
                R,
                S, R, L, L, R, S, S, S, S,
                R,
                S, S, S,
                R,
                F
            };

            Section.SectionTypes[] track4_TechnicalLoop =
            {
                G, G,
                S, R, L, L, R, S,
                R,
                S, R, L, L, R, S,
                R,
                S, S, S, S, S, S,
                R,
                S, R, L, L, R, S,
                R,
                F
            };

            Section.SectionTypes[] track5_LongSnake =
            {
                G, G,
                S, S, R, L, L, R, S, S,
                R,
                S, S,
                R,
                S, R, L, L, R, S, R, L, L, R, S,
                R,
                S, S,
                R,
                F
            };

            Competition.Tracks.Enqueue(new Track("Wide Oval", track1_WideOval));
            Competition.Tracks.Enqueue(new Track("Chicane Loop", track2_ChicaneLoop));
            Competition.Tracks.Enqueue(new Track("Double Bend", track3_DoubleBend));
            Competition.Tracks.Enqueue(new Track("Technical Loop", track4_TechnicalLoop));
            Competition.Tracks.Enqueue(new Track("Long Snake", track5_LongSnake));
        }

        public static void NextRace()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                // Geen console aanwezig
            }

            var next = Competition.NextTrack();

            if (next != null)
            {
                CurrentRace = new Race(next, Competition.Drivers);
                RaceChanged?.Invoke(null, EventArgs.Empty);
            }
            else
            {
                string result = "🏆 EINDKLASSEMENT 🏆\n\n";

                var standings = Competition.Drivers
                    .OrderByDescending(driver => driver.Points)
                    .ToList();

                for (int i = 0; i < standings.Count; i++)
                {
                    result += $"{i + 1}. {standings[i].Name} - {standings[i].Points} punten\n";
                }

                result += $"\nKAMPIOEN: {standings[0].Name}!";

                ChampionshipFinished?.Invoke(result);

                CurrentRace = null;
                RaceChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
