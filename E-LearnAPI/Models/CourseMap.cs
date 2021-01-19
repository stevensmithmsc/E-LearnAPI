using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    public class CourseMap
    {
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }
    }
}