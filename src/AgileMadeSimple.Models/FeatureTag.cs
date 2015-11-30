using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class FeatureTag
    {
        public int FeatureTagID { get; set; }
        public int FeatureID { get; set; }
        public int TagID { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
