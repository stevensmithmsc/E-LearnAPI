using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.DTOs
{
    /// <summary>
    /// The data needed to create a new e-Learning result in the database.
    /// </summary>
    public class NewResultDTO
    {
        /// <summary>
        /// This is the person's ESR Employee number.
        /// </summary>
        public int? Employee { get; set; }

        /// <summary>
        /// This is the person's username from the LMS.
        /// It should match their e-mail address.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// This is the name of the course/module from the LMS
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// This is the date the person completed this course/module.
        /// </summary>
        public DateTime CompletionDate { get; set; }
        
        /// <summary>
        /// The source of the e-Learning result, if left blank, the ADAccount prooviding the message will be used.
        /// </summary>
        public string Source { get; set; }

    }
}