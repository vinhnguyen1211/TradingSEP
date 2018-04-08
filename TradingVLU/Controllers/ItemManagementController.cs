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
            return View();
        }

        [HttpPost]
        public ActionResult add(HttpPostedFileBase[] uploadImages)
        {
            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {
                if (uploadImages != null && uploadImages.Count() >= 1)
                {
                    foreach (HttpPostedFileBase img in uploadImages)
                    {
                        if (img != null)
                        {
                            string[] n = img.FileName.Split('.');
                            string sfile = n[0] + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." +n[1];
                            string spath = Server.MapPath("~/Content/img/items/");
                            string sfullpath = Path.Combine(spath, sfile);
                            try
                            {
                                img.SaveAs(sfullpath);
                                db.item_images.Add(new item_images {
                                    item_id = 1,
                                    filename = sfile,
                                    path = sfullpath,
                                    create_by = "vinh",
                                    create_date = DateTime.Now,
                                    update_by = "vinh",
                                    update_date = DateTime.Now
                                });
                                db.SaveChanges();
                            }

                            catch (Exception ex)
                            {

                            }
                        }


                    }

                }
            }
            
            
            return View();
        }

    }
}