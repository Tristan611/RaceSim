using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {

        public List<Driver> Drivers { get; set; }
        public Queue<Track> Tracks { get; set; }
        public Track CurrentRaceTrack { get; set; }



        public Competition()
        {
            Drivers = new List<Driver>();
            Tracks = new Queue<Track>();
        }

        public Track NextTrack()
        {
            if (Tracks.Count == 0)
            {
                return null;
            }

            CurrentRaceTrack = Tracks.Dequeue();
            return CurrentRaceTrack;
        }


    }
}
