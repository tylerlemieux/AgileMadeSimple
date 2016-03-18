using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class Story
    {
        public int StoryID { get; set; }
        public string AcceptanceCriteria { get; set; }
        public string Blocked { get; set; }
        public string BlockedText { get; set; }
        public string Description { get; set; }
        public int EpicID { get; set; }
        public int? FeatureID { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public int? OwnerID { get; set; }
        public int? Points { get; set; }
        public int? SprintID { get; set; }
        public int StateID { get; set; }

        public virtual Epic Epic { get; set; }
        public virtual User Owner { get; set; }
        public virtual Sprint Sprint { get; set; }
        public virtual States State { get; set; }
    }
}
