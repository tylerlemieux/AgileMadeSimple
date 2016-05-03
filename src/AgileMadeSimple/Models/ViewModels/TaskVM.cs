using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileMadeSimple.Models.ViewModels
{
    public class TaskVM
    {
        public int TaskID { get; set; }
        public string Blocked { get; set; }
        public string BlockedMessage { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int? SprintID { get; set; }
        public int StateID { get; set; }
        public int StoryID { get; set; }
        public decimal? ToDoHours { get; set; }
        public decimal? TotalHours { get; set; }
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
