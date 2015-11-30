using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class StoryTag
    {
        public int StoryTagID { get; set; }
        public int StoryID { get; set; }
        public int TagID { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
