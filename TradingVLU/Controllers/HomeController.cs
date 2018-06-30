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
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                List<tempshoppingcart> tempcart = db.tempshoppingcarts.Where(x => x.buyer_id == userID).ToList();
                ViewBag.Cart = tempcart;
                ViewBag.CartUnits = tempcart.Count();
                decimal? temp = tempcart.Sum(c => c.quantity * c.price);
                decimal myDecimal = temp ?? 0;
                ViewBag.CartTotalPrice = myDecimal;
            }
            else
            {

            }
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
            var itemsz = db.items.Where(x => x.item_name.ToLower().Contains(text.ToLower()) && x.approve == 1).ToList();

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