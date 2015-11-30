using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class Tag
    {
        public Tag()
        {
            FeatureTag = new HashSet<FeatureTag>();
            StoryTag = new HashSet<StoryTag>();
        }

        public int TagID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FeatureTag> FeatureTag { get; set; }
        public virtual ICollection<StoryTag> StoryTag { get; set; }
    }
}
