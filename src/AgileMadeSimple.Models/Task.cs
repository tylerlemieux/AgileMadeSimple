using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class Task
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

        public virtual Story Story { get; set; }
    }
}
