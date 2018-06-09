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
            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var itemList = db.items.Where(x=>x.approve==1).Select(x => new { x.id, x.item_name, x.price, x.index_image}).ToList();
                ViewBag.itemList = itemList;

            }
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

        [Route("Order/{id:int:min(1)}")]
        public ActionResult Order(string id)
        {
            if (Session["userID"] != null)
            {
                int ID = int.Parse(id);
                int userID = int.Parse(Session["userID"].ToString());
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    var item = db.items.FirstOrDefault(x => x.id == ID);
                    var username = db.users.FirstOrDefault(x => x.id == userID).name;
                    //item.status = 2;
                    Order nOrder = new Order();
                    nOrder.item_id = ID;
                    nOrder.user_id = userID;
                    nOrder.status = 0;
                    //
                    nOrder.item_name = item.item_name;
                    nOrder.username = username;
                    //
                    db.Orders.Add(nOrder);
                    db.SaveChanges();
                }
                return RedirectToAction("index", "item");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }


        public ActionResult listitem()
        {

            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    var model = db.Orders.Where(x => x.item.seller_id == userID).ToList();
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [Route("Accept/{id:int:min(1)}")]
        public ActionResult Accept(string id)
        {
            int ID = int.Parse(id);
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var model = db.Orders.FirstOrDefault(x => x.item_id == ID);
                if (model.status != 1)
                {
                    model.status = 1;
                }               
                db.SaveChanges();
                return RedirectToAction("listitem", "Item");
            }
        }
        [Route("Reject/{id:int:min(1)}")]
        public ActionResult Reject(string id)
        {
            int ID = int.Parse(id);
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var model = db.Orders.FirstOrDefault(x => x.item_id == ID);
                if (model.status != 2)
                {
                    model.status = 2;
                }
                db.SaveChanges();
                return RedirectToAction("listitem", "Item");
            }
        }

    }
}