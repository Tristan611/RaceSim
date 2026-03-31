using Controller;
using RaceSimulator;
using static Controller.Data;
using static RaceSimulator.Visualize;
using System.Threading;

Controller.Data.Initialize();
Visualize.DrawTrack(CurrentRace.Track);
CurrentRace.DriversChanged += DriversChangedHandler;

Controller.Data.RaceChanged += (s, e) =>
{
    CurrentRace.DriversChanged += DriversChangedHandler;
    Visualize.DrawTrack(CurrentRace.Track);
};

for (; ; )
{
    CurrentRace.Update();
    Thread.Sleep(200);
}