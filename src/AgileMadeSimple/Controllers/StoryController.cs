using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgileMadeSimple.Models;
using AgileMadeSimple.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgileMadeSimple.Controllers
{
    [Route("api/[controller]")]
    public class StoryController : Controller
    {
        public enum StoryStates
        {
            BACKLOG,
            DEFINED,
            IN_PROGRESS,
            READY_FOR_TEST,
            COMPLETED,
            ACCEPTED

        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<Story> Get()
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Story.Select(s => s).ToArray();
            }
        }

        [HttpGet("Project/{epicId}")]
        public IEnumerable<Story> GetProjectStories(int epicId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Story.Where(e => e.EpicID == epicId && e.SprintID == null).Select(s => s).OrderBy(s => s.Order).ToList();
            }
        }

        // GET api/values/5
        [HttpGet("{storyId}")]
        public Story GetStory(int storyId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Story.Where(s => s.StoryID == storyId).First();
            }
        }

        // POST api/values
        [HttpPost]
        public Story Post([FromBody]Story story)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                int? maxOrderNumber = context.Story.Where(s => s.EpicID == story.EpicID).Select(s => s.Order).Max();
                story.Order = maxOrderNumber == null ? 0 : maxOrderNumber + 1;
                context.Story.Add(story);
                context.SaveChanges();
                return story;
            }
        }

        // PUT api/values/5
        [HttpPut]
        public IEnumerable<Story> Put([FromBody]Story story)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                context.Update(story);
                context.SaveChanges();
                //return context.Story.Where(s => s.EpicID == story.EpicID).ToList();
                return GetProjectStories(story.EpicID);
            }
        }

        [HttpPut("BulkEdit")]
        public void Put([FromBody]IEnumerable<Story> stories)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                context.UpdateRange(stories);
                context.SaveChanges();
            }
        }

        [HttpPut("AddTag/{storyId}/{tagName}")]
        public void AddTag(int storyId, string tagName)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Tag tag = TagHandler.CreateOrEditTag(tagName, TagHandler.TagType.STORY, storyId);
            }
        }

        [HttpDelete("RemoveTag/{storyId}/{tagName}")]
        public void RemoveTag(int storyId, string tagName)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                IEnumerable<StoryTag> storyTag =
                    from st in context.StoryTag
                    join t in context.Tag on st.TagID equals t.TagID
                    where storyId == st.StoryID && t.Name == tagName
                    select st;

                context.StoryTag.RemoveRange(storyTag);
                context.SaveChanges();
            }
        }


        // DELETE api/values/5
        [HttpDelete("{storyId}")]
        public void Delete(int storyId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Story story = context.Story.Where(s => s.StoryID == storyId).First();
                IEnumerable<StoryTag> storyTag = context.StoryTag.Where(st => st.StoryID == storyId).Select(st => st);
                context.Story.Remove(story);
                context.StoryTag.RemoveRange(storyTag);
                context.SaveChanges();
            }
        }

        #region Sprint Controller Actions
        [HttpPost("Sprint")]
        public IEnumerable<SprintsVM> CreateSprint([FromBody] Sprint sprint)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                context.Sprint.Add(sprint);
                context.SaveChanges();
                return (from s in context.Sprint
                        where s.ProjectID == sprint.ProjectID
                        select new SprintsVM
                        {
                            SprintID = s.SprintID,
                            DefinitionOfDone = s.DefinitionOfDone,
                            SprintGoals = s.SprintGoals,
                            StartDate = s.StartDate,
                            EndDate = s.EndDate,
                            ProjectID = s.ProjectID,
                            Stories =
                                (from st in context.Story
                                 where st.SprintID == s.SprintID
                                 select st).ToList()

                        }).ToList();
            }
        }

        [HttpGet("Sprints/{projectId}")]
        public IEnumerable<SprintsVM> GetSprints(int projectId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                var sprints =
                    (from s in context.Sprint
                     where s.ProjectID == projectId
                     select new SprintsVM
                     {
                         SprintID = s.SprintID,
                         DefinitionOfDone = s.DefinitionOfDone,
                         SprintGoals = s.SprintGoals,
                         StartDate = s.StartDate,
                         EndDate = s.EndDate,
                         ProjectID = s.ProjectID,
                         Stories =
                             (from st in context.Story
                              where st.SprintID == s.SprintID
                              select st).ToList()
                            
                    }).ToList();
                
                return sprints;

            }
        }

        #endregion
    }
}
