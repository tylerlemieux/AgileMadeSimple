using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileMadeSimple.Models.ViewModels
{
    public class RegistrationVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ResponseMessage { get; set; }
    }
}
