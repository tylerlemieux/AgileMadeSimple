using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileMadeSimple.Models.ViewModels
{
    public class SprintWallVM
    {
        public int SprintID { get; set; }
        public string DefinitionOfDone { get; set; }
        public string SprintGoals { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectID { get; set; }
        public IEnumerable<StoryVM> Story {get;set;}
    }
}
