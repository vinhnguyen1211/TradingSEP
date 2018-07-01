using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TradingVLU.Models;

namespace TradingVLU.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        vlutrading3545Entities db = new vlutrading3545Entities();
        // GET: Admin/User
        public ActionResult Index() 
        {
            var user = db.users.OrderByDescending(x => x.id).ToList();
            return View(user);
        }
        public ActionResult Delete(int id)
        {
            var user = db.users.FirstOrDefault(x => x.id == id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new UserDB().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpGet]
        public ActionResult logout()
        {
            Session["userLogged"] = null;
            return RedirectToAction("index", "Home", new { area = "" });
        }

        [HttpGet]
        public ActionResult setting()
        {
            return RedirectToAction("account_settings", "User", new { area = "" });
        }

    }
}