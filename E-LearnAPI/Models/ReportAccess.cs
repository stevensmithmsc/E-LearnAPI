using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    /// <summary>
    /// This represents the fields needed by the API and it's logic relating the the report access table
    /// in the database. This does not include all the fields in the database.
    /// </summary>
    public class ReportAccess
    {
        /// <summary>
        /// This is the ID of the linked staff record.  It also functions are the primary key.
        /// </summary>
        public int StaffID { get; set; }
        public virtual Person Staff { get; set; }

        /// <summary>
        /// This is the Access Level for the E-Learning API.
        /// 0 - Basic Access, 1 - Admin Access, 2 - Manager Access (allowed to delete records).
        /// </summary>
        public byte AccessLevel { get; set; }
    }
}