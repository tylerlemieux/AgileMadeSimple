﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgileMadeSimple.Models;
using Microsoft.AspNet.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgileMadeSimple.Controllers
{
    [Route("api/[controller]")]
    public class EpicController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<Epic> Get()
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                int[] userTeams = TeamModel.GetUserTeams(HttpContext.Session.GetInt32("UserID"));



                return context.Epic.Where(e => userTeams.Contains(e.TeamID)).Select(s => s).ToList();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Epic Get(int id)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Epic.Where(e => e.EpicID == id).First();
            }
        }
        // POST api/values
        [HttpPost]
        public Epic Post([FromBody]Epic epic)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                epic.StateID = 2;
                context.Epic.Add(epic);
                context.SaveChanges();
                return epic;
            }
        }

        [HttpPut("Assign/{epicId}")]
        public void AssignTeam(int epicId, [FromBody] int teamId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Epic epic = context.Epic.Where(e => e.EpicID == epicId).Select(e => e).First();
                epic.TeamID = teamId;
                context.Epic.Update(epic);
                context.SaveChanges();
            }
        }

        [HttpPut("Status/{epicId}")]
        public void ChangeStatus(int epicId, [FromBody] int stateId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Epic epic = context.Epic.Where(e => e.EpicID == epicId).Select(e => e).First();
                epic.StateID = stateId;
                context.Epic.Update(epic);
                context.SaveChanges();

            }
        }

        // PUT api/values/5
        [HttpPut("{epicId}")]
        public Epic Put(int epicId, [FromBody]Epic epic)
        {
            using(AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                context.Epic.Update(epic);
                context.SaveChanges();
                return epic;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{epicId}")]
        public void Delete(int epicId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Epic epic = context.Epic.Where(e => e.EpicID == epicId).First();
                context.Epic.Remove(epic);
                context.SaveChanges();
            }
        }

        [HttpGet("States/{teamId}")]
        public IEnumerable<States> getEpicStates(int teamId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                IEnumerable<States> states = context.States.Where(s => s.Type == "Epic" && teamId == s.TeamID).OrderBy(s => s.Order).Select(s => s);
                return states.ToList();
            }
        }

        [HttpPost("States/{teamId}")]
        public IEnumerable<States> addEpicState([FromBody] States state, int teamId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                state.Type = "Epic";
                context.States.Add(state);

                return context.States.Where(s => s.Type == "Epic" && teamId == s.TeamID).OrderBy(s => s.Order).Select(s => s).ToList();
            }
        }

        [HttpDelete("States/{stateId}")]
        public IEnumerable<States> deleteEpicState(int stateId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                States state = context.States.Where(s => s.StateID == stateId).First();
                context.States.Remove(state);

                return context.States.Where(s => s.Type == "Epic").OrderBy(s => s.Order).Select(s => s).ToList();
            }
        }

        [HttpGet("CurrentSprint/{projectId}")]
        public int? GetCurrentSprint(int projectId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                DateTime currentDateTime = DateTime.Now;
                return context.Sprint.Where(s => s.StartDate < currentDateTime 
                                          && s.EndDate > currentDateTime)
                              .Select(s => s.SprintID)
                              .FirstOrDefault();
            }
        }
    }
}
