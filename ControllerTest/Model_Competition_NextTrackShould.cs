using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);   // Test of de lijst null mag zijn
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Section.SectionTypes[] section1 = { Section.SectionTypes.StartGrid, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.Straight, Section.SectionTypes.Straight, Section.SectionTypes.Finish };
            Track track = new Track("Hoogeveen", section1);
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            Assert.AreEqual(track, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Section.SectionTypes[] section1 = { Section.SectionTypes.StartGrid, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.Straight, Section.SectionTypes.Straight, Section.SectionTypes.Finish };

            Track track1 = new Track("Hoogeveen", section1);
            _competition.Tracks.Enqueue(track1);

            var result = _competition.NextTrack();
            var result2 = _competition.NextTrack();
            Assert.IsNull(result2);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Section.SectionTypes[] section1 = { Section.SectionTypes.StartGrid, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.Straight, Section.SectionTypes.Straight, Section.SectionTypes.Finish };
            Section.SectionTypes[] section2 = { Section.SectionTypes.StartGrid, Section.SectionTypes.LeftCorner, Section.SectionTypes.LeftCorner, Section.SectionTypes.Straight, Section.SectionTypes.Straight, Section.SectionTypes.Finish };
            Track track1 = new Track("Hoogeveen", section1);
            Track track2 = new Track("Meppel", section2);
            _competition.Tracks.Enqueue(track1);
            _competition.Tracks.Enqueue(track2);
            var result = _competition.Tracks.Count;
            _competition.NextTrack();
            var result2 = _competition.Tracks.Count;
            _competition.NextTrack();
            var result3 = _competition.Tracks.Count;
            Assert.AreEqual(2, result);
            Assert.AreEqual(1, result2);
            Assert.AreEqual(0, result3);

        }

    }
}
