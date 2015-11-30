using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class TaskTag
    {
        public int TaskTagID { get; set; }
        public int TagID { get; set; }
        public int TaskID { get; set; }
    }
}
