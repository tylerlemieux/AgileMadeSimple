using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileMadeSimple.Models.ViewModels
{
    public class StoryVM
    {
        public int StoryID { get; set; }
        public string AcceptanceCriteria { get; set; }
        public string Blocked { get; set; }
        public string BlockedText { get; set; }
        public string Description { get; set; }
        public int EpicID { get; set; }
        public int? FeatureID { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public int? OwnerID { get; set; }
        public int? Points { get; set; }
        public int? SprintID { get; set; }
        public int StateID { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<TaskVM> Task { get; set; }
    }
}
