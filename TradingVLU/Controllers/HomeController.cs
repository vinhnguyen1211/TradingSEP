using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradingVLU.Models;
namespace TradingVLU.Controllers
{
    public class HomeController : Controller
    {
        vlutrading3545Entities db = new vlutrading3545Entities();
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
        [HttpPost]
        public ActionResult Search(string Search)
        {
            var product = db.items.Where(x => x.item_name.ToLower().Contains(Search.ToLower())).ToList();

            if (product.Count() > 0)
            {
                ViewBag.Message = "  ";

            }
            else
            {
                ViewBag.Message = "No Item found";

            }
            ViewData["project"] = product;
            return View();
        }
    }
}