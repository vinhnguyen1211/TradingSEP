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
        // GET: Base
        public BaseController()
        {
            ViewBag.CartTotalPrice = CartTotalPrice;
            ViewBag.Cart = Cart;
            ViewBag.CartUnits = Cart.Count;
        }

        private List<tempshoppingcart> Cart
        {
            get
            {
                return db.tempshoppingcarts.ToList();
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