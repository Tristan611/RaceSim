using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ControllerTest;

[TestFixture]
public class Model_TrackShould
{
    [Test]
    public void Constructor_SetName()
    {
        Section.SectionTypes[] sections =
        {
            Section.SectionTypes.StartGrid,
            Section.SectionTypes.Straight,
            Section.SectionTypes.Finish
        };

        var track = new Track("Testbaan", sections);

        Assert.AreEqual("Testbaan", track.Name);
    }

    [Test]
    public void Constructor_ConvertSectionTypesToSections()
    {
        Section.SectionTypes[] sections =
        {
            Section.SectionTypes.StartGrid,
            Section.SectionTypes.Straight,
            Section.SectionTypes.Finish
        };

        var track = new Track("Testbaan", sections);

        Assert.AreEqual(3, track.Sections.Count);
        Assert.AreEqual(Section.SectionTypes.StartGrid, track.Sections.First.Value.SectionType);
        Assert.AreEqual(Section.SectionTypes.Finish, track.Sections.Last.Value.SectionType);
    }
}
