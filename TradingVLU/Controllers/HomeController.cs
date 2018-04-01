﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradingVLU.Controllers
{
    public class HomeController : Controller
    {
        [Route]
        public ActionResult index()
        {
            return View();
        }

        [Route("about")]
        public ActionResult about()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Route("contact")]
        public ActionResult contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult items()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}