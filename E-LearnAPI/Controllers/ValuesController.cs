using E_LearnAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace E_LearnAPI.Controllers
{
    /// <summary>
    /// Gets data from database specific to current user.
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ValuesController : ApiController
    {
        private TrainingDatabase db = new TrainingDatabase();

        /// <summary>
        /// Gets the E-Learning Access Level for the current user.
        /// </summary>
        /// <returns></returns>
        // GET api/values
        public byte Get()
        {
            var userAccess = db.People.SingleOrDefault(p => p.ADAccount == User.Identity.Name);
            if (userAccess != null && userAccess.ReportAccess != null)
            {
                byte AccLvl = userAccess.ReportAccess.AccessLevel;
                return AccLvl;
            }
            return 0;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
