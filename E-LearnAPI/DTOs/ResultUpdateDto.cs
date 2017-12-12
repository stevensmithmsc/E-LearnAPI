using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.DTOs
{
    public class ResultUpdateDto
    {
        public int Id { get; set; }

        public bool Processed { get; set; }

        public string Comments { get; set; }
    }
}