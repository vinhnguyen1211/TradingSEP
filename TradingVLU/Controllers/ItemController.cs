using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
                var cmt = db.comments.Where(x => x.id_item == id);
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



        [HttpGet]
        public ActionResult list()
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var itemslist = db.items.ToList();
                return View(itemslist);
            }
           
        } 

        public ActionResult Comments(string masp, string cmt)
        {

            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                int ID = int.Parse(masp);                
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    var name = db.users.FirstOrDefault(x => x.id == userID).name;
                    comment nCom = new comment();
                    nCom.id_item = ID;
                    nCom.comment_txt = cmt;
                    nCom.id_user = userID;
                    nCom.name_comment = name;
                    db.comments.Add(nCom);
                    db.SaveChanges();
                }
                return RedirectToAction("detail", "Item", new { id = ID });
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [Route("AddToCart/{id:int:min(1)}")]
        public ActionResult AddToCart(int id)
        {
            vlutrading3545Entities dbc = new vlutrading3545Entities();
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    // check if product is valid
                    item product = db.items.FirstOrDefault(p => p.id == id);
                    var name = db.users.FirstOrDefault(x => x.id == userID);
                    if (product != null)
                    {
                        // check if product already existed
                        tempshoppingcart cart = db.tempshoppingcarts.FirstOrDefault(c => c.item_id == id);
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
                                buyer_id = userID,
                                buyer_name = name.name,
                                quantity = 1
                            };
                            db.tempshoppingcarts.Add(cart);
                        }                        
                        //product.UnitsInStock--;
                        db.SaveChanges();
                    }
                }
                
                //addToCart(id);
                //return RedirectToAction("Index");
                return RedirectToAction("index", "item");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        // GET: Checkout
        public ActionResult CheckoutIndex()
        {
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                var db = new vlutrading3545Entities();
                ViewBag.Cart = db.tempshoppingcarts.Where(x => x.buyer_id == userID).ToList<tempshoppingcart>();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        //[Route("ClearCarts/{id:int:min(1)}")]
        public ActionResult ClearCart()
        {
            var db = new vlutrading3545Entities();
            try
            {
                List<tempshoppingcart> carts = db.tempshoppingcarts.ToList();
                carts.ForEach(a => {
                    item product = db.items.FirstOrDefault(p => p.id == a.item_id);
                });
                db.tempshoppingcarts.RemoveRange(carts);
                db.SaveChanges();
            }
            catch (Exception) { }
            return RedirectToAction("index", "item");
        }

        [Route("RemoveCartItems/{id:int:min(1)}")]
        public ActionResult RemoveCartItem(int id)
        {
            vlutrading3545Entities db = new vlutrading3545Entities();
            tempshoppingcart product = db.tempshoppingcarts.FirstOrDefault(p => p.item_id == id);
            db.tempshoppingcarts.Remove(product);
            db.SaveChanges();
            return RedirectToAction("CheckoutIndex", "item");
        }


        public ActionResult Order()
        {
            int userID = int.Parse(Session["userID"].ToString());
            vlutrading3545Entities db = new vlutrading3545Entities();
            var name = db.users.FirstOrDefault(x => x.id == userID);
            order o = new order
            {
                orderdate = DateTime.Now,
                buyerid = userID,
                order_status = 0,
                buyer_name = name.name
            };
            db.orders.Add(o);

            foreach (var i in db.tempshoppingcarts.ToList<tempshoppingcart>())
            {
                db.order_detail.Add(new order_detail
                {
                    orderid = o.orderid,
                    item_id = i.item_id,
                    quantity = i.quantity,
                    totalprice = i.quantity * i.price,
                    item_status = 0,
                    item_orderdate = o.orderdate,
                    item_name = i.item_name,
                    buyer_name = i.buyer_name
                });
                db.tempshoppingcarts.Remove(i);
            }
            db.SaveChanges();
            return RedirectToAction("OrderSuccess");
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }

        public ActionResult confirmorderlist()
        {
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var model = db.order_detail.Where(x => x.item.seller_id == userID).ToList();
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        [Route("Accept/{orid:int:min(1)}/{itemid:int:min(1)}")]
        public ActionResult Accept(string orid, string itemid)
        {
            int ItemID = int.Parse(itemid);
            int OrderID = int.Parse(orid);
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var model = db.order_detail.FirstOrDefault(x => x.item_id == ItemID && x.orderid == OrderID);
                if (model.item_status != 1)
                {
                    model.item_status = 1;
                }
                db.SaveChanges();
                return RedirectToAction("confirmorderlist", "Item");
            }
        }

        [Route("Reject/{orid:int:min(1)}/{itemid:int:min(1)}")]
        public ActionResult Reject(string orid, string itemid)
        {
            int ItemID = int.Parse(itemid);
            int OrderID = int.Parse(orid);
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var model = db.order_detail.FirstOrDefault(x => x.item_id == ItemID && x.orderid == OrderID);
                if (model.item_status != 2)
                {
                    model.item_status = 2;
                }
                db.SaveChanges();
                return RedirectToAction("confirmorderlist", "Item");
            }
        }

        [Route("ChangeStatusToSoldOut/{itemid:int:min(1)}")]
        public ActionResult ChangeStatusToSoldOut(string itemid)
        {
            int ItemID = int.Parse(itemid);
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var model = db.items.FirstOrDefault(x => x.id == ItemID) ;
                if (model.approve != 3)
                {
                    model.approve = 3;
                }
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        //private int x = (int)System.Web.HttpContext.Current.Session["userID"];
        // GET: Base
        //public ItemController()
        //{
        //    ViewBag.CartTotalPrice = CartTotalPrice;
        //    ViewBag.Cart = Cart;
        //    ViewBag.CartUnits = Cart.Count;
        //}

        //private List<tempshoppingcart> Cart
        //{
        //    get
        //    {
        //        vlutrading3545Entities db = new vlutrading3545Entities();
        //        if (Session["userID"] != null)
        //        {
        //            int userID = int.Parse(Session["userID"].ToString());
        //            return db.tempshoppingcarts.Where(x => userID == x.buyer_id).ToList();
        //        }
        //        return null;
        //    }
        //}

        //private decimal CartTotalPrice
        //{
        //    get
        //    {
        //        decimal? temp = Cart.Sum(c => c.quantity * c.price);
        //        decimal myDecimal = temp ?? 0;
        //        return myDecimal;
        //    }

        //}


        public ActionResult checkoldorder()
        {
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var model = db.orders.Where(x => x.buyerid == userID).ToList();
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [Route("CheckDetailOrder/{orid:int:min(1)}")]
        public ActionResult CheckDetailOrder(string orid)
        {
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                int OrderID = int.Parse(orid);
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var model = db.order_detail.Where(x => x.orderid == OrderID).ToList();
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

    }
}