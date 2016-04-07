using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgileMadeSimple.Models;
using Microsoft.Data.Entity;

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

        [HttpGet("Sprint/{sprintId}")]
        public Sprint GetSprintData(int sprintId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                var sprint = context.Sprint.Where(s => s.SprintID == sprintId).Select(s => s).First();

                sprint.Story = context.Story.Include(s => s.Task).Where(s => s.SprintID == sprintId).Select(s => s).ToArray();

                return sprint;
            }
        }
    }
}
