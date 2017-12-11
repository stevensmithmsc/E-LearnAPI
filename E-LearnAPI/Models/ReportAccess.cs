using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    public class ReportAccess
    {
        public int StaffID { get; set; }
        public virtual Person Staff { get; set; }

        public short AccessLevel { get; set; }
    }
}