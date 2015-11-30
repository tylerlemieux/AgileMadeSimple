using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class User
    {
        public User()
        {
            Story = new HashSet<Story>();
            TeamUser = new HashSet<TeamUser>();
        }

        public int UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Story> Story { get; set; }
        public virtual ICollection<TeamUser> TeamUser { get; set; }
    }
}
