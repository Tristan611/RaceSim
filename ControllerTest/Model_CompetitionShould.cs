using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ControllerTest;

[TestFixture]
public class Model_CompetitionShould
{
    private Competition _competition;

    [SetUp]
    public void SetUp()
    {
        _competition = new Competition();
    }

    private Track CreateTrack(string name)
    {
        Section.SectionTypes[] sections =
        {
            Section.SectionTypes.StartGrid,
            Section.SectionTypes.Straight,
            Section.SectionTypes.Finish
        };

        return new Track(name, sections);
    }

    [Test]
    public void NextTrack_EmptyQueue_ReturnNull()
    {
        var result = _competition.NextTrack();

        Assert.IsNull(result);
    }

    [Test]
    public void NextTrack_OneInQueue_ReturnTrack()
    {
        var track = CreateTrack("Hoogeveen");

        _competition.Tracks.Enqueue(track);

        var result = _competition.NextTrack();

        Assert.AreEqual(track, result);
    }

    [Test]
    public void NextTrack_OneInQueue_RemoveTrackFromQueue()
    {
        var track = CreateTrack("Hoogeveen");

        _competition.Tracks.Enqueue(track);
        _competition.NextTrack();

        var result = _competition.NextTrack();

        Assert.IsNull(result);
    }

    [Test]
    public void NextTrack_TwoInQueue_ReturnTracksInCorrectOrder()
    {
        var track1 = CreateTrack("Hoogeveen");
        var track2 = CreateTrack("Meppel");

        _competition.Tracks.Enqueue(track1);
        _competition.Tracks.Enqueue(track2);

        var result1 = _competition.NextTrack();
        var result2 = _competition.NextTrack();

        Assert.AreEqual(track1, result1);
        Assert.AreEqual(track2, result2);
    }
}