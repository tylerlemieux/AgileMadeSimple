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
    public class TagController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            using (AgileMadeSimpleContext context = new AgileMadeSimpleContext())
            {
                //TODO... make this a group by and order it by the most 
                //instances of the tag to limit number sent for performance
                //Also would be nice to have this be paginated
                return context.Tag.Select(s => s.Name).ToArray();
            }
        }

        
    }
}
