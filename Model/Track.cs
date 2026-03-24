using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        
        public Track(string name, Section.SectionTypes[] sections)
        {
            this.Name = name;
            this.Sections = Conferter(sections);
        }

        public LinkedList<Section> Conferter(Section.SectionTypes[] sections)
        {
            LinkedList<Section> Sections = new LinkedList<Section>();
            foreach (var section in sections)
            {
                Sections.AddLast(new Section(section));
            }

            return Sections;
        }
    }
}
