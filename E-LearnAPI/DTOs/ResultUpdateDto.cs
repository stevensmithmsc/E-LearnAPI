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
        /// <summary>
        /// The ID of the result to be updated.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// If set to true - will assume any processing required has been done manually.
        /// If set to false - will attempt to process the record and update a requirement if one exists.
        /// </summary>
        public bool Processed { get; set; }
        /// <summary>
        /// The comments associated with the result.
        /// </summary>
        public string Comments { get; set; }
    }
}