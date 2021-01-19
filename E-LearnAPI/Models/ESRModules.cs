using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    public class ESRModules
    {
        public int ID { get; set; }
        public int Employee { get; set; }
        public int? StaffID { get; set; }
        public virtual Person Staff { get; set; }
        public string ModuleName { get; set; }
        public int? CourseID { get; set; }
        public virtual Course Course { get; set; }
        public DateTime CompletionDate { get; set; }
        public bool Processed { get; set; }
        public string Comments { get; set; }
    }
}