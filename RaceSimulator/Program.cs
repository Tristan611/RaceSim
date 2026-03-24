// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using Controller;
using Model;
using RaceSimulator;
using static Controller.Data;
using static RaceSimulator.Visualize;

Controller.Data.Initialize();
Visualize.DrawTrack(CurrentRace.Track);
//Visualize.MainLoop();
CurrentRace.DriversChanged += DriversChangedHandler;

Controller.Data.RaceChanged += (s, e) =>
{
    CurrentRace.DriversChanged += DriversChangedHandler;
    Visualize.DrawTrack(CurrentRace.Track);
};

for (; ; )
{ 
    Thread.Sleep(100);
}
