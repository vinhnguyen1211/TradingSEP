using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradingVLU.Models;

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
            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var ques = db.security_question.ToList();
                List<SelectListItem> item = new List<SelectListItem>();
                foreach (var i in ques)
                {
                    item.Add(new SelectListItem
                    {
                        Text = i.question,
                        Value = i.id.ToString()
                    });
                }

                ViewBag.question = item;
            }
            return View();
        }

        public ActionResult login()
        {
            return View();
        }

    }
}