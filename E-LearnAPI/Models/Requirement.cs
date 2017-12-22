using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    /// <summary>
    /// This represents the fields need by the API and it's logic from the requirements (req) table 
    /// in the database.  This does not include all the fields in the database.
    /// </summary>
    public class Requirement
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// This is the linked Staff (person) record - The person the requires the course.
        /// </summary>
        public int Staff { get; set; }
        public virtual Person StaffObject { get; set; }

        /// <summary>
        /// This is the linked Course record - The course the person requires.
        /// </summary>
        public int Course { get; set; }
        public virtual Course CourseObject { get; set; }

        /// <summary>
        /// This is a link to the status of the requirement.
        /// 1 - Required, 2 - Booked, 5 - Completed, 7 - No Longer Required
        /// </summary>
        public Nullable<short> Status { get; set; }
        public virtual Status StatusObject { get; set; }

        /// <summary>
        /// Any comments recorded with this requirement.
        /// </summary>
        public string Comments { get; set; }

    }
}