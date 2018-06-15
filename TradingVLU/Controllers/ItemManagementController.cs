using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradingVLU.Models;

namespace TradingVLU.Controllers
{
    [RoutePrefix("manageitem")]
    [Route("{action=index}")]
    public class ItemManagementController : Controller
    {
        // GET: ItemManagement

        public ActionResult index()
        {
            return View();
        }

        public ActionResult list()
        {
            return View();
        }

        public ActionResult add()
        {
            //if (Session["userLogged"] == null)
            //{
            //    return RedirectToAction("account_settings", "User");
            //}
            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var statusList = db.item_status.Select(x => new { x.id, x.status }).ToList();
                ViewBag.statusList = statusList;
            }
            return View();
        }
        [HttpGet]
        public ActionResult approve()
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var itemList = db.items.Where(x => x.approve == 0).Select(x => new { x.id, x.item_name, x.price, x.index_image,x.description }).ToList();

                ViewBag.itemList = itemList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult approve(int id)
        {
            String message;
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
               
                try
                {
                   
                    var item = db.items.Find(id);
                    item.approve = 1;
                    //db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "success";
                }
                catch(Exception ex)
                {
                    message = "failed";
                }
            }
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            
        }

        [HttpGet]
        public ActionResult myItems(int status = 0)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var itemList = db.items.Where(x => x.approve == status).Select(x => new { x.id, x.item_name, x.price, x.index_image, x.description }).ToList();

                ViewBag.itemList = itemList;
                ViewBag.status = status;
            }
            return View();

        }

      

        [HttpPost]
        public ActionResult reject(int id)
        {
            String message;
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {

                try
                {

                    var item = db.items.Find(id);
                    item.approve = 2;
                    //db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "success";
                }
                catch (Exception ex)
                {
                    message = "failed";
                }
            }
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult add(String name, String description, int quantity, int price, int status, HttpPostedFileBase index_image,
                                IEnumerable<HttpPostedFileBase> detail_images)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                //list status
                var statusList = db.item_status.Select(x => new { x.id, x.status }).ToList();
                ViewBag.statusList = statusList;
                //
                string index_img = String.Empty;
                if (index_image != null && index_image.ContentLength > 0)
                {
                    //img = new byte[index_image.ContentLength];
                    //using (BinaryReader read = new BinaryReader(index_image.InputStream))
                    //{
                    //    img = read.ReadBytes(index_image.ContentLength);
                    //}
                    //index_img_base64 = Convert.ToBase64String(img);
                    var fileName = Guid.NewGuid() + Path.GetExtension(index_image.FileName);
                    string RootFolder = @Server.MapPath("~/Content/img/items/index_img/");
                    string path = Path.Combine(RootFolder, fileName);
                    index_img = fileName;
                    index_image.SaveAs(path);
                }

                string[] detail_img = new string[5];
                int count = 0;
                if (detail_images != null)
                {
                    if(detail_images.Count() > 5)
                    {
                        ViewBag.ErrorMessage = "Error occurred while create new item. Contact Admin for details";
                        return View();
                    }

                    foreach (var file in detail_images)
                    {
                        detail_img[count] = String.Empty;
                        if (file != null && file.ContentLength > 0)
                        {
                            
                            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            string RootFolder = @Server.MapPath("~/Content/img/items/detail_img/");
                            string path = Path.Combine(RootFolder, fileName);
                            detail_img[count] = fileName;
                            file.SaveAs(path);
                            count++;
                        }
                    }
                }


                item item = new item
                {
                    item_name = name,
                    description = description,
                    quantity = quantity,
                    price = price,
                    status = status,
                    index_image = index_img,
                    seller_id = 1,
                    create_by = "vinh",
                    create_date = DateTime.Now,
                    update_by = "vinh",
                    update_date = DateTime.Now,
                    detail_image1 = detail_img[0],
                    detail_image2 = detail_img[1],
                    detail_image3 = detail_img[2],
                    detail_image4 = detail_img[3],
                    detail_image5 = detail_img[4],
                    approve=0
                };
                try
                {
                    db.items.Add(item);
                    
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = "Error occurred while create new item. Contact Admin for details";
                    return View();

                }
                //if (uploadImages != null)
                //{
                //    string[] n = uploadImages.FileName.Split('.');
                //    string sfile = n[0] + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + n[1];
                //    string spath = Server.MapPath("~/Content/img/items/");
                //    string sfullpath = Path.Combine(spath, sfile);
                //    try
                //    {
                //        uploadImages.SaveAs(sfullpath);
                //        db.item_images.Add(new item_images
                //        {
                //            item_id = 1,
                //            filename = sfile,
                //            path = sfullpath,
                //            create_by = "vinh",
                //            create_date = DateTime.Now,
                //            update_by = "vinh",
                //            update_date = DateTime.Now
                //        });
                //        db.SaveChanges();
                //    }

                //    catch (Exception ex)
                //    {

                //    }


                //}
            }




            return View();
        }
        [HttpGet]
        public ActionResult EditItem(int id)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var Data = db.items.FirstOrDefault(x => x.id == id);
                return View(Data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(int id, item its)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var Data = db.items.FirstOrDefault(x => x.id == id);
                Data.item_name = its.item_name;
                Data.description = its.description;
                Data.quantity = its.quantity;
                Data.status = its.status;
                db.SaveChanges();
                return RedirectToAction("Sale_ProjectsList");
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
            
            
