using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgileMadeSimple.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgileMadeSimple.Controllers
{
    [Route("api/[controller]")]
    public class FeatureController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<Feature> Get()
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Feature.Select(f => f);
            }

        }

        [HttpGet("ByEpic/{epicId}")]
        public IEnumerable<Feature> GetFeatureByEpic(int epicId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                return context.Feature.Where(f => f.EpicID == epicId).Select(f => f);
            }
        }

        // GET api/values/5
        [HttpGet("{featureId}")]
        public Feature Get(int featureId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Feature feature = context.Feature.Where(f => f.FeatureID == featureId).First();
                return feature;
            }

        }

        // POST api/values
        [HttpPost]
        public Feature Post([FromBody]Feature feature)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                context.Feature.Add(feature);
                context.SaveChanges();
                return feature;
            }
        }

        // PUT api/values/5
        [HttpPut("{featureId}")]
        public Feature Put(int featureId, [FromBody]Feature feature)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                context.Feature.Update(feature);
                context.SaveChanges();
                return feature;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{featureId}")]
        public void Delete(int featureId)
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                Feature feature = context.Feature.Where(f => f.FeatureID == featureId).First();
                IEnumerable<FeatureTag> featureTag = context.FeatureTag.Where(ft => ft.FeatureID == featureId).Select(ft => ft);
                context.Remove(feature);
                context.RemoveRange(featureTag);
                context.SaveChanges();
            }
        }
    }
}
