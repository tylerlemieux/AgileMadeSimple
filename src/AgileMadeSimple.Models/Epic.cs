using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class Epic
    {
        public Epic()
        {
            Feature = new HashSet<Feature>();
            Story = new HashSet<Story>();
        }

        public int EpicID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public int TeamID { get; set; }

        public virtual ICollection<Feature> Feature { get; set; }
        public virtual ICollection<Story> Story { get; set; }
        public virtual States State { get; set; }
        public virtual Team Team { get; set; }
    }
}
