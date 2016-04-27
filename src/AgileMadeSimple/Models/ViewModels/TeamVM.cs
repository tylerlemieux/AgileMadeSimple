using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileMadeSimple.Models.ViewModels
{
    public class TeamVM
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserVM> Users { get; set; }

    }

    public class UserVM
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
