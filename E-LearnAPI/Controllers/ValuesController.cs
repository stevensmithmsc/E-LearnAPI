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
        // GET api/values
        public string Get()
        {
            return User.Identity.Name;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        
    }
}
