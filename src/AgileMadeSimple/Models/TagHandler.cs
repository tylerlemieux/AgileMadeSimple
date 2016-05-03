using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileMadeSimple.Models
{
    public static class TagHandler
    {
        public static Tag GetTagID(string name)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Tag tag = context.Tag.Where(t => t.Name == name).FirstOrDefault();

                if (tag == null)
                {
                    //Tag doesnt exist, create a new one
                    tag = new Tag() { Name = name };
                    context.Add(tag);
                    context.SaveChanges();
                }

                return tag;
            }
        }

    }
}
