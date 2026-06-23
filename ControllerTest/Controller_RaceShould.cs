using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace ControllerTest;

[TestFixture]
public class Controller_RaceShould
{
    private Competition _competition;
    private Track _track;
    private List<Driver> _drivers;

    [SetUp]
    public void SetUp()
    {
        _competition = new Competition();

        Section.SectionTypes[] sections =
        {
            Section.SectionTypes.StartGrid,
            Section.SectionTypes.StartGrid,
            Section.SectionTypes.Straight,
            Section.SectionTypes.Finish
        };

        _track = new Track("Testbaan", sections);
        _competition.CurrentRaceTrack = _track;

        _drivers = new List<Driver>
        {
            new Driver("Mario", 0, IParticipant.TeamColors.Red, new Car(100, 10, 10, false), _competition),
            new Driver("Luigi", 0, IParticipant.TeamColors.Green, new Car(100, 10, 10, false), _competition),
            new Driver("Peach", 0, IParticipant.TeamColors.Blue, new Car(100, 10, 10, false), _competition),
            new Driver("Bowser", 0, IParticipant.TeamColors.Grey, new Car(100, 10, 10, false), _competition),
            new Driver("Wario", 0, IParticipant.TeamColors.Yellow, new Car(100, 10, 10, false), _competition)
        };

        _competition.Drivers = _drivers;
    }

    [Test]
    public void SetPlayerStartPositions_ResetRaceValues()
    {
        var race = new Race(_track, _drivers);

        foreach (var driver in _drivers)
        {
            Assert.AreEqual(0, driver.Lap);
            Assert.AreEqual(0, driver.RaceProgress);
            Assert.IsFalse(driver.Finish);
            Assert.IsFalse(driver.ChangedDriverPosition);
        }
    }

    [Test]
    public void SetPlayerStartPositions_MaxTwoDriversPerSection()
    {
        var race = new Race(_track, _drivers);

        var groupedDrivers = _drivers
            .GroupBy(driver => driver.PosOnTrack);

        foreach (var group in groupedDrivers)
        {
            Assert.LessOrEqual(group.Count(), 2);
        }
    }

    [Test]
    public void GivePoints_AddCorrectPointsToFinishedDrivers()
    {
        Data.Competition = _competition;

        var race = new Race(_track, _drivers);

        race.FinishedDrivers.AddRange(_drivers);

        race.GivePoints();

        Assert.AreEqual(10, _drivers[0].Points);
        Assert.AreEqual(8, _drivers[1].Points);
        Assert.AreEqual(6, _drivers[2].Points);
        Assert.AreEqual(4, _drivers[3].Points);
        Assert.AreEqual(2, _drivers[4].Points);
    }

    [Test]
    public void GivePoints_AddPointsOnTopOfExistingPoints()
    {
        Data.Competition = _competition;

        var race = new Race(_track, _drivers);

        _drivers[0].Points = 10;
        race.FinishedDrivers.Add(_drivers[0]);

        race.GivePoints();

        Assert.AreEqual(20, _drivers[0].Points);
    }

    [Test]
    public void CheckNextPosition_WhenDriverReachesThreeLaps_SetsFinishToTrue()
    {
        var race = new Race(_track, _drivers);
        var driver = _drivers[0];

        int totalTrackLength = _track.Sections.Count * 100;
        driver.RaceProgress = totalTrackLength * 3 - 1;

        driver.CheckNextposition();

        Assert.IsTrue(driver.Finish);
        Assert.AreEqual(totalTrackLength * 3, driver.RaceProgress);
    }

    [Test]
    public void GivePoints_AddsPointsToGenericPointsResults()
    {
        Data.Competition = _competition;

        var race = new Race(_track, _drivers);
        race.FinishedDrivers.AddRange(_drivers);

        race.GivePoints();

        Assert.AreEqual("Mario", _competition.PointsResults.GetBestParticipant());
    }

    [Test]
    public void GivePoints_WhenCalledMultipleTimes_AddsPointsTogetherInGenericPointsResults()
    {
        Data.Competition = _competition;

        var race = new Race(_track, _drivers);

        race.FinishedDrivers.Add(_drivers[0]);
        race.GivePoints();

        race.FinishedDrivers.Clear();

        race.FinishedDrivers.Add(_drivers[0]);
        race.GivePoints();

        var bestParticipant = _competition.PointsResults.GetBestParticipant();

        Assert.AreEqual("Mario", bestParticipant);
        Assert.AreEqual(20, _drivers[0].Points);
    }

    [Test]
    public void GivePoints_AddsTimeResultForFinishedDrivers()
    {
        Data.Competition = _competition;

        var race = new Race(_track, _drivers);
        race.FinishedDrivers.Add(_drivers[0]);

        race.GivePoints();

        Assert.AreEqual("Mario", _competition.TimeResults.GetBestParticipant());
    }

    [Test]
    public void TimeResults_GetBestParticipant_ReturnsFastestDriver()
    {
        var timeResults = new ResultData<TimeResult>();

        timeResults.Add(new TimeResult("Mario", TimeSpan.FromSeconds(10)));
        timeResults.Add(new TimeResult("Luigi", TimeSpan.FromSeconds(8)));

        var result = timeResults.GetBestParticipant();

        Assert.AreEqual("Luigi", result);
    }
}
