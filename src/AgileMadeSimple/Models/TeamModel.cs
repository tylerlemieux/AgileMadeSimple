using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AgileMadeSimple.Models
{
    public static class TeamModel
    {
        /// <summary>
        /// Checks that a user has rights to the team
        /// (if they are on the team)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public static bool UserHasRightsToTeam(int userId, int teamId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                bool userHasRights =
                    (from t in context.Team
                     join tu in context.TeamUser on t.TeamID equals tu.TeamID
                     where tu.UserID == userId && t.TeamID == teamId
                     select t).Count() > 0;

                return userHasRights;
            }
        }

        public static int[] GetUserTeams(int? userId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return
                    (from t in context.Team
                     join tu in context.TeamUser on t.TeamID equals tu.TeamID
                     where tu.UserID == userId
                     select t.TeamID).ToArray();
            }
        }
        
    }
}
