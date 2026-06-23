using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ControllerTest;

[TestFixture]
public class Model_DriverShould
{
    private Competition _competition;

    [SetUp]
    public void SetUp()
    {
        _competition = new Competition();
    }

    [Test]
    public void CalculateDriverSpeed_ReturnPerformanceTimesSpeed()
    {
        var car = new Car(100, 10, 12, false);
        var driver = new Driver("Mario", 0, IParticipant.TeamColors.Red, car, _competition);

        var result = driver.CalculateDriverSpeed();

        Assert.AreEqual(120, result);
    }

    [Test]
    public void ResetDrivers_ResetRaceValues()
    {
        var car = new Car(100, 10, 12, false);
        var driver = new Driver("Mario", 5, IParticipant.TeamColors.Red, car, _competition);

        driver.PosOnTrack = 5;
        driver.Lap = 2;
        driver.LeftOrRight = 1;
        driver.RaceProgress = 500;
        driver.Finish = true;

        driver.ResetDrivers();

        Assert.AreEqual(0, driver.PosOnTrack);
        Assert.AreEqual(0, driver.Lap);
        Assert.AreEqual(0, driver.LeftOrRight);
        Assert.AreEqual(0, driver.RaceProgress);
        Assert.IsFalse(driver.Finish);
    }
}
