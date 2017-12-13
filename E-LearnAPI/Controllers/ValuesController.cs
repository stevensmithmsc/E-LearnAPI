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
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ValuesController : ApiController
    {
        private TrainingDatabase db = new TrainingDatabase();

        // GET api/values
        public byte Get()
        {
            ReportAccess userAccess = db.People.SingleOrDefault(p => p.ADAccount == User.Identity.Name).ReportAccess;
            if (userAccess != null)
            {
                byte AccLvl = userAccess.AccessLevel;
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
