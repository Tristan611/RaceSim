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

while (CurrentRace != null)
{
    CurrentRace.Update();
    Thread.Sleep(200);
}

Console.WriteLine("Alle races zijn klaar!");