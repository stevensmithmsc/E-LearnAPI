using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    public class Person
    {
        public int ID { get; set; }

        public string Fname { get; set; }

        public string Sname { get; set; }

        public string ADAccount { get; set; }

        public virtual ICollection<Requirement> Requirements { get; set; }

        public Person()
        {
            this.Requirements = new HashSet<Requirement>();
        }
    }
}