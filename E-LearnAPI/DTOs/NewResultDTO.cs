using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearnAPI.DTOs
{
    public class NewResultDTO
    {
        /// <summary>
        /// This is the person's ESR Employee number.
        /// </summary>
        public int? Employee { get; set; }

        public string UserName { get; set; }

        public string ModuleName { get; set; }

        /// <summary>
        /// This is when the result was recieved by the API.
        /// </summary>
        public DateTime CompletionDate { get; set; }
        
        public string Source { get; set; }


    }
}