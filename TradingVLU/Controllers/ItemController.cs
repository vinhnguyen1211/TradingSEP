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
    public class ItemController : BaseController
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
                Session["Item"] = item.id;
                var cmt = db.Comments.Where(x => x.id_item == id);
                var count =cmt.Count();
                if(item == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    return HttpNotFound();
                }
                ViewBag.Count = count;
                ViewBag.DetailItem = item;
                ViewBag.cmt = cmt.ToList();
                ViewBag.IdItem = id;
                return View(cmt.ToList());
            }

        }

        public ActionResult list()
        {
            return View();
        }

        //[Route("Order/{id:int:min(1)}")]
        //public ActionResult Order(string id)
        //{
        //    if (Session["userID"] != null)
        //    {
        //        int ID = int.Parse(id);
        //        int userID = int.Parse(Session["userID"].ToString());
        //        using (vlutrading3545Entities db = new vlutrading3545Entities())
        //        {
        //            var item = db.items.FirstOrDefault(x => x.id == ID);
        //            var name = db.users.FirstOrDefault(x => x.id == userID).name;
        //            //item.status = 2;
        //            Order nOrder = new Order();
        //            nOrder.item_id = ID;
        //            nOrder.user_id = userID;           
        //            nOrder.status = 0;
        //            //
        //            nOrder.item_name = item.item_name;
        //            nOrder.name = name;
        //            //
        //            db.Orders.Add(nOrder);
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("index", "item");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "User");
        //    }
        //}

        public ActionResult listitem()
        {

            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    var model = db.order_detail.Where(x => x.item.seller_id == userID).ToList();
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        //[Route("Accept/{id:int:min(1)}")]
        //public ActionResult Accept(string id)
        //{
        //    int ID = int.Parse(id);
        //    using (vlutrading3545Entities db = new vlutrading3545Entities())
        //    {
        //        var model = db.Orders.FirstOrDefault(x => x.item_id == ID);
        //        if (model.status != 1)
        //        {
        //            model.status = 1;
        //        }               
        //        db.SaveChanges();
        //        return RedirectToAction("listitem", "Item");
        //    }
        //}
        //[Route("Reject/{id:int:min(1)}")]
        //public ActionResult Reject(string id)
        //{
        //    int ID = int.Parse(id);
        //    using (vlutrading3545Entities db = new vlutrading3545Entities())
        //    {
        //        var model = db.Orders.FirstOrDefault(x => x.item_id == ID);
        //        if (model.status != 2)
        //        {
        //            model.status = 2;
        //        }
        //        db.SaveChanges();
        //        return RedirectToAction("listitem", "Item");
        //    }
        //}

        //public ActionResult Showcmt()
        //{
        //    int ID = (int)Session["Item"];
        //    using (vlutrading3545Entities db = new vlutrading3545Entities())
        //    {
        //        var model = db.Comments.Where(x => x.id_item == ID).ToList();
        //        return PartialView(model);
        //    }
        //}

        public ActionResult Comments(string masp, string cmt)
        {

            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                int ID = int.Parse(masp);                
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    var name = db.users.FirstOrDefault(x => x.id == userID).name;
                    Comment nCom = new Comment();
                    nCom.id_item = ID;
                    nCom.comment1 = cmt;
                    nCom.id_user = userID;
                    nCom.name = name;
                    db.Comments.Add(nCom);
                    db.SaveChanges();
                }
                return RedirectToAction("detail", "Item", new { id = ID });
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [Route("Order/{id:int:min(1)}")]
        public ActionResult AddToCart(int id)
        {
            if (Session["userID"] != null)
            {
                addToCart(id);
                //return RedirectToAction("Index");
                return RedirectToAction("index", "item");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        private void addToCart(int pId)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                // check if product is valid
                item product = db.items.FirstOrDefault(p => p.id == pId);
                if (product != null)
                {
                    // check if product already existed
                    tempshoppingcart cart = db.tempshoppingcarts.FirstOrDefault(c => c.item_id == pId);
                    if (cart != null)
                    {
                        cart.quantity++;
                    }
                    else
                    {

                        cart = new tempshoppingcart
                        {
                            item_name = product.item_name,
                            item_id = product.id,
                            price = product.price,
                            quantity = 1
                        };

                        db.tempshoppingcarts.Add(cart);
                    }
                    //product.UnitsInStock--;
                    db.SaveChanges();
                }
            }
        }


    }
}