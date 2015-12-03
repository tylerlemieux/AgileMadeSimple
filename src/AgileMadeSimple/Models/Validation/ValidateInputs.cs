using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileMadeSimple.Models;
using AgileMadeSimple.Models.ViewModels;

namespace AgileMadeSimple.Models.Validation
{
    public static class ValidateInputs
    {
        public static ValidationResponse ValidateRegistration(RegistrationVM input)
        {
            ValidationResponse response = new ValidationResponse();
            response.Valid = true;

            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {

                if (input.Password != input.ConfirmPassword)
                {
                    response.Valid = false;
                    response.Message = "The two passwords do not match";
                }
                else if (context.User.Where(u => u.Username == input.Username).Count() > 0)
                {
                    response.Valid = false;
                    response.Message = "Username already exists";
                }
                else if (context.User.Where(u => u.Email == input.Email).Count() > 0)
                {
                    response.Valid = false;
                    response.Message = "Account is already registered with this email address";
                }
                //TODO add regex to check for valid email address

                return response;
            }
        }
    }
}
