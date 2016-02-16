﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgileMadeSimple.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgileMadeSimple.Controllers
{
    [Route("api/[controller]")]
    public class StoryController : Controller
    {
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
                return context.Story.Where(e => e.EpicID == epicId).Select(s => s).ToList();
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
                return context.Story.Where(s => s.EpicID == story.EpicID).ToList();
            }
        }

        [HttpPut("AddTag/{storyId}/{tagName}")]
        public void AddTag(int storyId, string tagName)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Tag tag = TagHandler.GetTagID(tagName);
                StoryTag storyTag = new StoryTag();
                storyTag.StoryID = storyId;
                storyTag.TagID = tag.TagID;
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
    }
}
