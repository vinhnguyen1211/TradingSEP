using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TradingVLU.Models;

namespace TradingVLU.Controllers
{
    [RoutePrefix("item")]
    [Route("{action=index}")]
    public class ItemController : Controller
    {
        public ActionResult index()
        {
            return View();
        }

        [Route("detail/{id:int:min(1)}")]
        public ActionResult detail(int id)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var item = db.items.FirstOrDefault(x => x.id == id);
                if(item == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    return HttpNotFound();
                }

                ViewBag.DetailItem = item;
                

            }
            ViewBag.IdItem = id;
            return View();
        }

        public ActionResult list()
        {
            return View();
        }
    }
}