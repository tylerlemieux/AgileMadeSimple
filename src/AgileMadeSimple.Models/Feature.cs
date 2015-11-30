using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class Feature
    {
        public Feature()
        {
            FeatureTag = new HashSet<FeatureTag>();
        }

        public int FeatureID { get; set; }
        public int EpicID { get; set; }
        public string Name { get; set; }
        public int? Points { get; set; }
        public int? ProgramID { get; set; }
        public int StateID { get; set; }

        public virtual ICollection<FeatureTag> FeatureTag { get; set; }
        public virtual Epic Epic { get; set; }
        public virtual States State { get; set; }
    }
}
