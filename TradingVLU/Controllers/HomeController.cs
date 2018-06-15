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

        [Route("search")]
        public ActionResult Search(string text)
        {
            var itemsz = db.items.Where(x => x.item_name.ToLower().Contains(text.ToLower())).ToList();

            if (itemsz.Count() > 0)
            {
                ViewBag.Message = "  ";

            }
            else
            {
                ViewBag.Message = "No Item found";

            }
            ViewData["Item"] = itemsz;
            return View(itemsz);
        }
    }
}