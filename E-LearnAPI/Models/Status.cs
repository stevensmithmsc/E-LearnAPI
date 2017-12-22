using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    /// <summary>
    /// This class represents a status record from the database.
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public short ID { get; set; }

        /// <summary>
        /// The description of the status.
        /// </summary>
        public string StatusDesc { get; set; }

        /// <summary>
        /// The requirements which have this status.
        /// </summary>
        public virtual ICollection<Requirement> Requirements { get; set; }

        public Status()
        {
            this.Requirements = new HashSet<Requirement>();
        }
    }
}