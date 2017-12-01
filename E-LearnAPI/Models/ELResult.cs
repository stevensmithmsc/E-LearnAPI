using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    public class ELResult
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public string PersonName { get; set; }

        public int CourseId { get; set; }

        public string CourseDesc { get; set; }

        public int Score { get; set; }

        public int? MaxScore { get; set; }

        public bool? PassFail { get; set; }

        public DateTime Received { get; set; }

        public string FromADAcc { get; set; }

        public bool Processed { get; set; }

        public string Comments { get; set; }
    }
}