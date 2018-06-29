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
            if (Session["userID"] != null)
            {
                vlutrading3545Entities db = new vlutrading3545Entities();
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

        public ActionResult list()
        {
            return View();
        }

        public ActionResult add()
        {
            if (Session["userLogged"] == null)
            {
                return RedirectToAction("account_settings", "User");
            }
            using (vlutrading3545Entities db = new vlutrading3545Entities())
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
                var itemList = db.items.Where(x => x.approve == 0).Select(x => new { x.id, x.item_name, x.price, x.index_image,x.description, x.quantity, x.phone_contact }).ToList();

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
                catch(Exception )
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
                int user_id = (int)Session["userID"];
                var itemList = db.items.Where(x => x.approve == status ).Where(x => x.seller_id == user_id).Select(x => new { x.id, x.item_name, x.price, x.index_image, x.description,x.quantity, x.phone_contact }).ToList();

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
                catch (Exception )
                {
                    message = "failed";
                }
            }
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult add(string name, string description, int quantity, int price, string phone, HttpPostedFileBase index_image,
                                IEnumerable<HttpPostedFileBase> detail_images)
        {

            if (Session["userLogged"] == null)
            {
                return RedirectToAction("account_settings", "User");
            }
            
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                //list status
                var statusList = db.item_status.Select(x => new { x.id, x.status }).ToList();
                ViewBag.statusList = statusList;
                //validate: require length of item name >= 10 characters
                if (name.Trim().Length < 10)
                {
                    ViewBag.ErrorMessage = "Name of item requires at least 10 characters";
                    return View();
                }
               
                
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

                var user = Session["userLogged"] as TradingVLU.Models.user;

                item item = new item
                {
                    item_name = name,
                    description = description,
                    quantity = quantity,
                    price = price,
                    phone_contact = phone,
                    status =1,
                    index_image = index_img,
                    seller_id = user.id,
                    create_by = user.name,
                    create_date = DateTime.Now,
                    update_by = user.name,
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
                catch (Exception )
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

            ViewBag.SuccessMessage = "Created new item successfully!";
            return View();
        }

        //[HttpGet]
        //public ActionResult EditItem(int id)
        //{
        //    using (vlutrading3545Entities db = new vlutrading3545Entities())
        //    {
        //        var Data = db.items.FirstOrDefault(x => x.id == id);
        //        return View(Data);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditItem(int id, item its)
        //{
        //    using (vlutrading3545Entities db = new vlutrading3545Entities())
        //    {
        //        var Data = db.items.FirstOrDefault(x => x.id == id);
        //        Data.item_name = its.item_name;
        //        Data.description = its.description;
        //        Data.quantity = its.quantity;
        //        Data.status = its.status;
        //        db.SaveChanges();
        //        return RedirectToAction("Sale_ProjectsList");
        //    }
        //}

        [HttpGet]
        public ActionResult edit(int id)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                //ViewBag.StatusSelect = db.item_status.Select(h => new SelectListItem { Value = h.id.ToString(), Text = h.status });
                var item = db.items.FirstOrDefault(x => x.id == id);
                ViewBag.DetailItem = item;
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult edit(int id, item nitem, HttpPostedFileBase avatar, HttpPostedFileBase pic01 , HttpPostedFileBase pic02 , HttpPostedFileBase pic03, HttpPostedFileBase pic04 , HttpPostedFileBase pic05)
        {
            vlutrading3545Entities db = new vlutrading3545Entities();
            int userID = int.Parse(Session["userID"].ToString());
            var username = db.users.FirstOrDefault(x => x.id == userID).name;
            var data = db.items.FirstOrDefault(x => x.id == nitem.id);

            data.price = nitem.price;
            data.description = nitem.description;
            data.item_name = nitem.item_name;
            data.description = nitem.description;
            data.phone_contact = nitem.phone_contact;
            data.quantity = nitem.quantity;
            data.update_by = username;

            string avatar_temp = null;
            if (avatar != null && avatar.ContentLength > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                string RootFolder = @Server.MapPath("~/Content/img/items/index_img/");
                string path = Path.Combine(RootFolder, fileName);
                avatar_temp = fileName;
                avatar.SaveAs(path);
            }
            if (avatar_temp != null)
            {
                data.index_image = avatar_temp;
            }

            string pic01_temp = null;
            if (pic01 != null && pic01.ContentLength > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(pic01.FileName);
                string RootFolder = @Server.MapPath("~/Content/img/items/detail_img/");
                string path = Path.Combine(RootFolder, fileName);
                pic01_temp = fileName;
                pic01.SaveAs(path);
            }
            if (pic01_temp != null)
            {
                data.detail_image1 = pic01_temp;
            }


            string pic02_temp = null;
            if (pic02 != null && pic02.ContentLength > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(pic02.FileName);
                string RootFolder = @Server.MapPath("~/Content/img/items/detail_img/");
                string path = Path.Combine(RootFolder, fileName);
                pic02_temp = fileName;
                pic02.SaveAs(path);
            }
            if (pic02_temp != null)
            {
                data.detail_image2 = pic02_temp;
            }

            string pic03_temp = null;
            if (pic03 != null && pic03.ContentLength > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(pic03.FileName);
                string RootFolder = @Server.MapPath("~/Content/img/items/detail_img/");
                string path = Path.Combine(RootFolder, fileName);
                pic03_temp = fileName;
                pic03.SaveAs(path);
            }
            if (pic03_temp != null)
            {
                data.detail_image3 = pic03_temp;
            }

            string pic04_temp = null;
            if (pic04 != null && pic04.ContentLength > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(pic04.FileName);
                string RootFolder = @Server.MapPath("~/Content/img/items/detail_img/");
                string path = Path.Combine(RootFolder, fileName);
                pic04_temp = fileName;
                pic04.SaveAs(path);
            }
            if (pic04_temp != null)
            {
                data.detail_image4 = pic04_temp;
            }

            string pic05_temp = null;
            if (pic05 != null && pic05.ContentLength > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(pic05.FileName);
                string RootFolder = @Server.MapPath("~/Content/img/items/detail_img/");
                string path = Path.Combine(RootFolder, fileName);
                pic05_temp = fileName;
                pic05.SaveAs(path);
            }
            if (pic05_temp != null)
            {
                data.detail_image5 = pic05_temp;
            }

            db.SaveChanges();
            return RedirectToAction("myItems", "ItemManagement");
        }

        //public string Images(item p)
        //{
        //    string s = "";
        //    string filename;
        //    string extension;
        //    filename = Path.GetFileNameWithoutExtension(p.avatar.FileName);
        //    extension = Path.GetExtension(p.avatar.FileName);
        //    filename = filename + extension;
        //    s = filename;
        //    filename = Path.Combine(Server.MapPath("/Content/img/items/index_img/"), filename);
        //    p.avatar.SaveAs(filename);
        //    return s;
        //}

        //if (index_image != null && index_image.ContentLength > 0)
        //        {
        //var fileName = Guid.NewGuid() + Path.GetExtension(index_image.FileName);
        //string RootFolder = @Server.MapPath("~/Content/img/items/index_img/");
        //string path = Path.Combine(RootFolder, fileName);
        //index_img = fileName;
        //            index_image.SaveAs(path);
        //        }


    //public string Images01(item p)
    //    {
    //        string s = "";
    //        string filename;
    //        string extension;
    //        filename = Path.GetFileNameWithoutExtension(p.pic01.FileName);
    //        extension = Path.GetExtension(p.pic01.FileName);
    //        filename = filename + extension;
    //        s = filename;
    //        filename = Path.Combine(Server.MapPath("/Content/img/items/detail_img/"), filename);
    //        p.pic01.SaveAs(filename);
    //        return s;
    //    }

    //    public string Images02(item p)
    //    {
    //        string s = "";
    //        string filename;
    //        string extension;
    //        filename = Path.GetFileNameWithoutExtension(p.pic02.FileName);
    //        extension = Path.GetExtension(p.pic02.FileName);
    //        filename = filename + extension;
    //        s = filename;
    //        filename = Path.Combine(Server.MapPath("/Content/img/items/detail_img/"), filename);
    //        p.pic02.SaveAs(filename);
    //        return s;
    //    }

    //    public string Images03(item p)
    //    {
    //        string s = "";
    //        string filename;
    //        string extension;
    //        filename = Path.GetFileNameWithoutExtension(p.pic03.FileName);
    //        extension = Path.GetExtension(p.pic03.FileName);
    //        filename = filename + extension;
    //        s = filename;
    //        filename = Path.Combine(Server.MapPath("/Content/img/items/detail_img/"), filename);
    //        p.pic03.SaveAs(filename);
    //        return s;
    //    }

    //    public string Images04(item p)
    //    {
    //        string s = "";
    //        string filename;
    //        string extension;
    //        filename = Path.GetFileNameWithoutExtension(p.pic04.FileName);
    //        extension = Path.GetExtension(p.pic04.FileName);
    //        filename = filename + extension;
    //        s = filename;
    //        filename = Path.Combine(Server.MapPath("/Content/img/items/detail_img/"), filename);
    //        p.pic04.SaveAs(filename);
    //        return s;
    //    }

        //    [NonAction]
        //public string Images05(item p)
        //{
        //    string s = "";
        //    string filename;
        //    string extension;
        //    filename = Path.GetFileNameWithoutExtension(p.pic05.FileName);
        //    extension = Path.GetExtension(p.pic05.FileName);
        //    filename = filename + extension;
        //    s = filename;
        //    filename = Path.Combine(Server.MapPath("/Content/img/items/detail_img/"), filename);
        //    p.pic05.SaveAs(filename);
        //    return s;
        //}

    }
}

            
            
