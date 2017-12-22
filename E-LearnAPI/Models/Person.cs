using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    /// <summary>
    /// This represents the fields needed by the API and it's logic relating to the Person (Staff member)
    /// table in the database.  This does not include all the fields in the database.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Forename
        /// </summary>
        public string Fname { get; set; }

        /// <summary>
        /// Surname
        /// </summary>
        public string Sname { get; set; }

        /// <summary>
        /// ESR Employee Number
        /// </summary>
        public int? ESRID { get; set; }

        /// <summary>
        /// AD Account
        /// </summary>
        public string ADAccount { get; set; }

        /// <summary>
        /// The requirements linked to this staff record.
        /// </summary>
        public virtual ICollection<Requirement> Requirements { get; set; }

        /// <summary>
        /// The report access record linked to this staff record.
        /// </summary>
        public virtual ReportAccess ReportAccess { get; set; }

        public Person()
        {
            this.Requirements = new HashSet<Requirement>();
        }
    }
}