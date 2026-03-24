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
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }
        
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
            //Section.SectionTypes[] section1 = { Section.SectionTypes.StartGrid, Section.SectionTypes.StartGrid, Section.SectionTypes.RightCorner,Section.SectionTypes.Straight,Section.SectionTypes.Straight,Section.SectionTypes.Straight,Section.SectionTypes.LeftCorner, Section.SectionTypes.RightCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.Straight,Section.SectionTypes.Straight, Section.SectionTypes.Straight,Section.SectionTypes.Straight,Section.SectionTypes.RightCorner, Section.SectionTypes.Straight, Section.SectionTypes.Straight, Section.SectionTypes.Straight, Section.SectionTypes.Straight, Section.SectionTypes.RightCorner, Section.SectionTypes.Finish};
            Section.SectionTypes[] section2 = { Section.SectionTypes.StartGrid, Section.SectionTypes.StartGrid, Section.SectionTypes.StartGrid,Section.SectionTypes.LeftCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.LeftCorner,Section.SectionTypes.LeftCorner, Section.SectionTypes.RightCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.Straight,Section.SectionTypes.Straight, Section.SectionTypes.RightCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.LeftCorner,Section.SectionTypes.LeftCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.RightCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.LeftCorner,Section.SectionTypes.LeftCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.RightCorner, Section.SectionTypes.LeftCorner,Section.SectionTypes.LeftCorner,Section.SectionTypes.RightCorner,Section.SectionTypes.RightCorner, Section.SectionTypes.Straight, Section.SectionTypes.RightCorner,Section.SectionTypes.Straight, Section.SectionTypes.Finish };
            Section.SectionTypes[] section3 = { Section.SectionTypes.StartGrid, Section.SectionTypes.StartGrid, Section.SectionTypes.StartGrid, Section.SectionTypes.LeftCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.Straight, Section.SectionTypes.Straight, Section.SectionTypes.RightCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.RightCorner, Section.SectionTypes.Straight, Section.SectionTypes.RightCorner, Section.SectionTypes.Straight, Section.SectionTypes.Finish };

            //Track track1 = new("Rainbow Road", section1);
            Track track2 = new("Coconut Mall", section2);
            Track track3 = new("Rainbow Road2", section3);

            //Competition.Tracks.Enqueue(track1);
            Competition.Tracks.Enqueue(track2);
            Competition.Tracks.Enqueue(track3);


        }

        public static void NextRace()
        {
            Console.Clear();
            var Next = Competition.NextTrack();
            if(Next != null)
            {
                CurrentRace = new Race(Next, Competition.Drivers);
            }
        }
    }
}
