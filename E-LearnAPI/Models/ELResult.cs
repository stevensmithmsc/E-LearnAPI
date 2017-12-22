﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models
{
    /// <summary>
    /// This class represents the E-Learning result that will be stored in the database.
    /// </summary>
    public class ELResult
    {
        /// <summary>
        /// Primary Key - Autogenerated by database.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// This is the person's ESR Employee number.
        /// </summary>
        public int PersonId { get; set; }

        public string PersonName { get; set; }
        /// <summary>
        /// This the the ID field from the Courses table.
        /// </summary>
        public int CourseId { get; set; }

        public string CourseDesc { get; set; }

        public int Score { get; set; }

        public int? MaxScore { get; set; }
        /// <summary>
        /// true = Pass, false = Fail
        /// </summary>
        public bool? PassFail { get; set; }
        /// <summary>
        /// This is when the result was recieved by the API.
        /// </summary>
        public DateTime Received { get; set; }
        /// <summary>
        /// This is the AD Account that sent the result message.
        /// Will only be populated when message does not come from cross origin location.
        /// </summary>
        public string FromADAcc { get; set; }
        /// <summary>
        /// Will represent if the result has been matched to a requirement and the requirement 
        /// updated based on the result.
        /// </summary>
        public bool Processed { get; set; }
        /// <summary>
        /// If application is unable to process result, an explanation will be put into this field.
        /// May be updated manually by admin staff.
        /// </summary>
        public string Comments { get; set; }
    }
}