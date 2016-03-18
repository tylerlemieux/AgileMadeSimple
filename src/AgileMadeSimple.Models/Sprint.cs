using System;
using System.Collections.Generic;

namespace AgileMadeSimple.Models
{
    public partial class Sprint
    {
        public Sprint()
        {
            Story = new HashSet<Story>();
        }

        public int SprintID { get; set; }
        public string DefinitionOfDone { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectID { get; set; }
        public string SprintGoals { get; set; }
        public DateTime StartDate { get; set; }

        public virtual ICollection<Story> Story { get; set; }
    }
}
