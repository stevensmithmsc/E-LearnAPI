using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.DTOs
{
    /// <summary>
    /// Represents the data needed to update a E-Learning Result, 
    /// only allows processed and comments fields to be updated.
    /// </summary>
    public class ResultUpdateDto
    {
        public int Id { get; set; }

        public bool Processed { get; set; }

        public string Comments { get; set; }
    }
}