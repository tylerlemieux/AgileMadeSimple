﻿using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgileMadeSimple.Models;
using Microsoft.Data.Entity;
using AgileMadeSimple.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgileMadeSimple.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Task.Select(t => t);
            }
        }

        // GET api/values/5
        [HttpGet("{taskId}")]
        public Task Get(int taskId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Task.Where(t => t.TaskID == taskId).First();
            }
        }

        [HttpGet("StoryTask/{storyId}")]
        public IEnumerable<Task> GetStoryTasks(int storyId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Task.Where(t => t.StoryID == storyId).Select(t => t);
            }
        }


        // POST api/values
        [HttpPost]
        public Task Post([FromBody]Task task)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                context.Task.Add(task);
                context.SaveChanges();
                return task;
            }
        }

        // PUT api/values/5
        [HttpPut]
        public Task Put([FromBody]Task task)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                context.Task.Update(task);
                context.SaveChanges();
                return task;
            }
        }

        [HttpPut("AddTag/{taskId}/{tagName}")]
        public void AddTag(int taskId, string tagName)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Tag tag = TagHandler.GetTagID(tagName);
                TaskTag taskTag = new TaskTag();
                taskTag.TaskID = taskId;
                taskTag.TagID = tag.TagID;
            }
        }

        [HttpDelete("RemoveTag/{taskId}/{tagName}")]
        public void RemoveTag(int taskId, string tagName)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                IEnumerable<TaskTag> taskTag =
                    from tt in context.TaskTag
                    join t in context.Tag on tt.TagID equals t.TagID
                    where taskId == tt.TaskID && t.Name == tagName
                    select tt;

                context.TaskTag.RemoveRange(taskTag);
                context.SaveChanges();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{taskId}")]
        public void Delete(int taskId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Task task = context.Task.Where(t => t.TaskID == taskId).First();
                IEnumerable<TaskTag> taskTag = context.TaskTag.Where(tt => tt.TaskID == taskId).Select(tt => tt);
                context.Task.Remove(task);
                context.TaskTag.RemoveRange(taskTag);
                context.SaveChanges();
            }
        }

        //[HttpGet("Sprint/{sprintId}")]
        //public Sprint GetSprintData(int sprintId)
        //{
        //    using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
        //    {
        //        var sprint = context.Sprint.Where(s => s.SprintID == sprintId).Select(s => s).First();

        //        sprint.Story = context.Story.Include(s => s.Task).Where(s => s.SprintID == sprintId).Select(s => s).ToArray();

        //        return sprint;
        //    }
        //}

        [HttpGet("Sprint/{sprintId}")]
        public SprintWallVM GetSprint(int sprintId)
        {
            AgileMadeSimpleContext context = new AgileMadeSimpleContext();

            //var sprint = 
            //    from s in context.Sprint
            //    join story in context.Story.Include(s => s.Task) on s.SprintID equals story.SprintID
            //    where s.SprintID == sprintId
            //    group s by new { s.SprintID, s.ProjectID, s.DefinitionOfDone, s.StartDate, s.EndDate, s.SprintGoals } into groupedSprints

            //    select new SprintsVM
            //    {
            //        SprintID = groupedSprints.Key.SprintID,
            //        ProjectID = groupedSprints.Key.ProjectID,
            //        DefinitionOfDone = groupedSprints.Key.DefinitionOfDone,
            //        StartDate = groupedSprints.Key.StartDate,
            //        EndDate = groupedSprints.Key.EndDate,
            //        SprintGoals = groupedSprints.Key.SprintGoals,
            //        Stories = groupedSprints


            //    }



            var sprint = context.Sprint
                .Where(s => s.SprintID == sprintId)
                .Select(s => new SprintWallVM
                {
                    SprintID = s.SprintID,
                    ProjectID = s.ProjectID,
                    DefinitionOfDone = s.DefinitionOfDone,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    SprintGoals = s.SprintGoals
                }).First();


            sprint.Story = context.Story.Where(s => s.SprintID == sprintId)
                .Select(s => new StoryVM {
                    SprintID = s.SprintID,
                    AcceptanceCriteria = s.AcceptanceCriteria,
                    StateID = s.StateID,
                    StoryID = s.StoryID,
                    Blocked = s.Blocked,
                    BlockedText = s.BlockedText,
                    Description = s.Description,
                    EpicID = s.EpicID,
                    FeatureID = s.FeatureID,
                    Name = s.Name,
                    Order = s.Order, 
                    OwnerID = s.OwnerID,
                    Points = s.Points
                }).ToArray();

            foreach(var story in sprint.Story)
            {
                story.Task = context.Task.Where(t => t.StoryID == story.StoryID)
                    .Select(t => new TaskVM {
                        SprintID = t.SprintID,
                        StateID = t.StateID,
                        StoryID = t.StoryID,
                        Blocked = t.Blocked,
                        BlockedMessage = t.BlockedMessage,
                        Description = t.Description,
                        Name = t.Name, 
                        TaskID = t.TaskID,
                        ToDoHours = t.ToDoHours,
                        TotalHours = t.TotalHours
                    }).ToArray();
            }

            return sprint;
            
        }

         
    }
}
