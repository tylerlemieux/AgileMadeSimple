using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgileMadeSimple.Models;
using AgileMadeSimple.Models.ViewModels;
using AgileMadeSimple.Models.Validation;
using Microsoft.AspNet.Session;
using Microsoft.AspNet.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgileMadeSimple.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpPost("Register")]
        public User Register([FromBody]RegistrationVM userInfo)
        {
            ValidationResponse validationResponse = ValidateInputs.ValidateRegistration(userInfo);
            if (validationResponse.Valid)
            {
                using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
                {
                    User user = new User();
                    user.Salt = Authentication.CreateSalt(25);
                    user.Name = userInfo.Name;
                    user.Username = userInfo.Username;
                    user.Password = Authentication.CalculatePasswordHash(userInfo.Password, user.Salt);
                    user.Email = userInfo.Email;

                    context.User.Add(user);
                    context.SaveChanges();

                    HttpContext.Session.SetInt32("UserID", user.UserID);

                    return user;
                }
            }
            else
            {
                throw new Exception(validationResponse.Message);
            }

            
        }

        [HttpPost("Login")]
        public User Login([FromBody]LoginVM credentials)
        {
            const string failedLoginMessage = "Your username or password was incorrect";

            using(AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                if(context.User.Where(u => u.Username == credentials.Username).Count() > 0)
                {
                    //Make sure username exists... then check the password
                    int userId = context.User.Where(u => u.Username == credentials.Username).Select(s => s.UserID).First();
                    bool authenticated = Authentication.ComparePassword(credentials.Password, userId);

                    if (authenticated)
                    {
                        HttpContext.Session.SetInt32("UserID", userId);
                        return context.User.First(u => u.UserID == userId);
                    }
                    else
                    {
                        throw new Exception(failedLoginMessage);
                    }

                }
                else
                {
                    throw new Exception(failedLoginMessage);
                }
            }
        }

        [HttpDelete("Logout")]
        public void Logout()
        {
            HttpContext.Session.Clear();
        }

        [HttpGet("CurrentUser")]
        public User GetCurrentUser()
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                return userId == null ? null : context.User.Where(u => u.UserID == userId).First();
            }
        }

    }
}
