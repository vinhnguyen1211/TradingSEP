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
        [HttpPost, ValidateInput(false)]
        public ActionResult index(String name, String description, int quantity, int status, HttpPostedFileBase uploadImages)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                byte[] img = null;
                string img_as_string = "";
                if (uploadImages != null)
                {
                    img = new byte[uploadImages.ContentLength];
                    using (BinaryReader read = new BinaryReader(uploadImages.InputStream))
                    {
                        img = read.ReadBytes(uploadImages.ContentLength);
                    }
                    img_as_string = Convert.ToBase64String(img);
                }
                item item = new item
                {
                    item_name = name,
                    description = description,
                    quantity = quantity,
                    status = status,
                    images = img_as_string,
                    seller_id = 1,
                    create_by = "vinh",
                    create_date = DateTime.Now,
                    update_by = "vinh",
                    update_date = DateTime.Now
                };
                try
                {
                    db.items.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.DuplicateMessage = "Error occurred while create new item. Contact Admin for details";
                    return View();
                    throw;
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

        public ActionResult list()
        {
            return View();
        }

        public ActionResult add()
        {
            return View();
        }

        [Route("detail/{id:int:min(1)}")]
        public ActionResult detail(int id)
        {
            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var item = db.items.FirstOrDefault(x => x.id == id);
                var imgList = db.item_images.Where(x => x.item_id == id)
                                            .Select(x => new { x.filename, imgString = x.base64_string })
                                            .ToList();

                ViewBag.DetailItem = item;
                ViewBag.imgList = imgList;
                
            }
            ViewBag.IdItem = id;
            return View();
        }
            
            
            
        

    }
}