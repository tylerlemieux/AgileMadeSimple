using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class TeamUser
    {
        public int TeamUserID { get; set; }
        public int TeamID { get; set; }
        public int UserID { get; set; }

        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
