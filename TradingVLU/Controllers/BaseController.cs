using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradingVLU.Models;

namespace TradingVLU.Controllers
{
    public class BaseController : Controller
    {
        vlutrading3545Entities db = new vlutrading3545Entities();
        public int x;
        // GET: Base
        public BaseController()
        {
            x = (int)System.Web.HttpContext.Current.Session["userID"];
            ViewBag.CartTotalPrice = CartTotalPrice;
            ViewBag.Cart = Cart;
            ViewBag.CartUnits = Cart.Count;
        }

        private List<tempshoppingcart> Cart
        {
            get
            {
                vlutrading3545Entities db = new vlutrading3545Entities();
                if (x != -1)
                {
                    int userID = x;
                    return db.tempshoppingcarts.Where(x => userID == x.buyer_id).ToList();
                }
                return null;
            }
        }

        private decimal CartTotalPrice
        {
            get
            {
                decimal? temp = Cart.Sum(c => c.quantity * c.price);
                decimal myDecimal = temp ?? 0;
                return myDecimal;
            }
            
        }
    }
}