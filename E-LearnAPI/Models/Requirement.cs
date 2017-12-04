using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    public class Requirement
    {
        public int ID { get; set; }

        public int Staff { get; set; }
        public virtual Person StaffObject { get; set; }

        public int Course { get; set; }
        public virtual Course CourseObject { get; set; }

        public Nullable<short> Status { get; set; }
        public virtual Status StatusObject { get; set; }

        public string Comments { get; set; }

    }
}