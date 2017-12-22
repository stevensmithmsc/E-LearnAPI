using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    /// <summary>
    /// Represents the fields needed by the API and it's logic relating to the Course table
    /// in the database.  This does not include all the fields in the database.
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The name of the course.
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Is this a Paris course?
        /// </summary>
        public bool Paris { get; set; }

        /// <summary>
        /// Is this a Child Health course?
        /// </summary>
        public bool Child_Health { get; set; }

        /// <summary>
        /// The requirement records linked to this course.
        /// </summary>
        public virtual ICollection<Requirement> Requirements { get; set; }

        public Course()
        {
            this.Requirements = new HashSet<Requirement>();
        }
    }
}