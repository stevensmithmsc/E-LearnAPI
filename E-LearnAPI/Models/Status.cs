using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    public class Status
    {
        public short ID { get; set; }

        public string StatusDesc { get; set; }

        public virtual ICollection<Requirement> Requirements { get; set; }

        public Status()
        {
            this.Requirements = new HashSet<Requirement>();
        }
    }
}