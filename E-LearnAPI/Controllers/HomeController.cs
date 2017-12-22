﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_LearnAPI.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Serves the home page of the web site.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "E-Learning Results Processing";

            return View();
        }
    }
}
