using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradingVLU.Controllers
{
    [RoutePrefix("users")]
    [Route("{action=index}")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult register()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }

    }
}