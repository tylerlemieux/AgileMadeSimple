using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileMadeSimple.Models
{   
    public static class TagHandler
    {
        public enum TagType {TASK, STORY, FEATURE};
        public static Tag CreateOrEditTag(string name, TagType type, int id)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Tag tag = context.Tag.Where(t => t.Name == name).FirstOrDefault();

                if (tag == null)
                {
                    //Tag doesnt exist, create a new one
                    tag = new Tag() { Name = name };
                    context.Add(tag);
                }
                //Todo redo this to use <T> as a type variable
                switch (type)
                {
                    case TagType.TASK:
                        TaskTag taskTag = new TaskTag()
                        {
                            TagID = tag.TagID,
                            TaskID = id
                        };

                        context.TaskTag.Add(taskTag);
                        break;
                    case TagType.STORY:
                        StoryTag storyTag = new StoryTag()
                        {
                            TagID = tag.TagID,
                            StoryID = id
                        };

                        context.StoryTag.Add(storyTag);
                        break;
                    case TagType.FEATURE:
                        FeatureTag featureTag = new FeatureTag()
                        {
                            TagID = tag.TagID,
                            FeatureID = id
                        };

                        context.FeatureTag.Add(featureTag);
                        break;
                }
                context.SaveChanges();

                return tag;
            }
           
        }

    }
}
