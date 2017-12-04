using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    public class Course
    {
        public int ID { get; set; }

        public string CourseName { get; set; }

        public bool Paris { get; set; }

        public bool Child_Health { get; set; }

        public virtual ICollection<Requirement> Requirements { get; set; }

        public Course()
        {
            this.Requirements = new HashSet<Requirement>();
        }
    }
}