using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class TaskUser
    {
        public int TaskUserID { get; set; }
        public int TaskID { get; set; }
        public int UserID { get; set; }
    }
}
