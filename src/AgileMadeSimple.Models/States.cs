using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class States
    {
        public States()
        {
            Epic = new HashSet<Epic>();
            Feature = new HashSet<Feature>();
            Story = new HashSet<Story>();
        }

        public int StateID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int TeamID { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Epic> Epic { get; set; }
        public virtual ICollection<Feature> Feature { get; set; }
        public virtual ICollection<Story> Story { get; set; }
        public virtual Team Team { get; set; }
    }
}
