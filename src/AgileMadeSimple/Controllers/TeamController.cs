using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgileMadeSimple.Models;
using Microsoft.AspNet.Http;
using AgileMadeSimple.Models.ViewModels;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgileMadeSimple.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<TeamVM> Get()
        {
            AgileMadeSimpleContext context = new AgileMadeSimpleContext();

            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                HttpContext.Response.StatusCode = 403;
                return null;
            }

            var teams =
                (from team in context.Team
                 join tu in context.TeamUser on team.TeamID equals tu.TeamID
                 join user in context.User on tu.UserID equals user.UserID
                 where user.UserID == userId
                 select new TeamVM
                 {
                     TeamID = team.TeamID,
                     Name = team.Name,
                 }).Distinct().ToArray();

            foreach (var team in teams)
            {
                team.Users =
                    (from u in context.User
                     join ut in context.TeamUser on u.UserID equals ut.UserID
                     where ut.TeamID == team.TeamID
                     select new UserVM {
                         UserID = u.UserID,
                         Name = u.Name,
                         Username = u.Username,
                         Email = u.Email
                     }).ToArray();
            }

            return teams;

        }

        [HttpGet("Project/{projectId}")]
        public IEnumerable<UserVM> GetProjectTeam(int projectId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                var team =
                    (from t in context.Team
                     join e in context.Epic on t.TeamID equals e.TeamID
                     join tu in context.TeamUser on t.TeamID equals tu.TeamID
                     join u in context.User on tu.UserID equals u.UserID
                     where e.EpicID == projectId
                     select new UserVM
                     {
                        UserID = u.UserID,
                        Username = u.Username,
                        Email = u.Email,
                        Name = u.Name
                     }).ToArray();

                return team;
            }
        }

        // POST api/values
        [HttpPost("Member/{teamId}/{username}")]
        public IEnumerable<TeamVM> Post(int teamId, string username)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                //check the user is allowed to add to the group
                //current rule = must be in the group
                int? activeUserId = HttpContext.Session.GetInt32("UserID");
                if (activeUserId == null)
                {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    return null;
                }
                else
                {
                    bool userHasRights = TeamModel.UserHasRightsToTeam((int)activeUserId, teamId);
                    int userId = context.User.Where(u => u.Username == username).Select(u => u.UserID).First();

                    bool userNotAlreadyInTeam =
                        (from t in context.Team
                         join tu in context.TeamUser on t.TeamID equals tu.TeamID
                         where tu.UserID == userId && t.TeamID == teamId
                         select t).Count() == 0;

                    if (userHasRights)
                    {
                        if (userNotAlreadyInTeam)
                        {
                            TeamUser teamUser = new TeamUser
                            {
                                TeamID = teamId,
                                UserID = userId
                            };

                            context.TeamUser.Add(teamUser);
                            context.SaveChanges();

                            return Get();
                        }
                        else
                        {
                            Response.StatusCode = StatusCodes.Status409Conflict;
                            return null;
                        }
                    }
                    else
                    {
                        Response.StatusCode = StatusCodes.Status403Forbidden;
                        return null;
                    }
                }

            }
        }

        // PUT api/values/5
        [HttpPost("{teamName}")]
        public IEnumerable<TeamVM> CreateTeam(string teamName)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return null;
            }

            AgileMadeSimpleContext context = new AgileMadeSimpleContext();
            Team teamToAdd = new Team() { Name = teamName };
            context.Team.Add(teamToAdd);
            context.SaveChanges();
            context.TeamUser.Add(new TeamUser() { UserID = (int)userId, TeamID = teamToAdd.TeamID});
            context.SaveChanges();
            return Get();
        }

        // DELETE api/values/5
        [HttpDelete("Member/{teamId}/{userId}")]
        public IEnumerable<TeamVM> DeleteMember(int teamId, int userId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                int? activeUserId = HttpContext.Session.GetInt32("UserID");

                if (activeUserId == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return null;
                }

                bool userHasRights = TeamModel.UserHasRightsToTeam((int)activeUserId, teamId);
                if (userHasRights)
                {
                    TeamUser teamUser = context.TeamUser.Where(t => t.TeamID == teamId && userId == t.UserID).Select(s => s).First();
                    context.TeamUser.Remove(teamUser);
                    context.SaveChanges();
                }
                else
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return null;
                }
            }

            return Get();
        }

        [HttpDelete("{teamId}")]
        public IEnumerable<TeamVM> Delete(int teamId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                int? userId = HttpContext.Session.GetInt32("UserID");

                if (userId == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return null;
                }

                bool userHasRights = TeamModel.UserHasRightsToTeam((int)userId, teamId);
                if (userHasRights)
                {
                    var teamUsers = context.TeamUser.Where(t => t.TeamID == teamId).Select(s => s);
                    context.TeamUser.RemoveRange(teamUsers);
                    Team team = context.Team.Where(t => t.TeamID == teamId).Select(s => s).First();
                    context.Team.Remove(team);
                    context.SaveChanges();
                }
                else
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return null;
                }
            }

            return Get();
        }
    }
}
