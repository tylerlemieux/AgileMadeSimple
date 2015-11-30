using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class Team
    {
        public Team()
        {
            Epic = new HashSet<Epic>();
            States = new HashSet<States>();
            TeamUser = new HashSet<TeamUser>();
        }

        public int TeamID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Epic> Epic { get; set; }
        public virtual ICollection<States> States { get; set; }
        public virtual ICollection<TeamUser> TeamUser { get; set; }
    }
}
