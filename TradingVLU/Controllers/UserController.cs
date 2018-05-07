using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TradingVLU.Models;

namespace TradingVLU.Controllers
{
    [RoutePrefix("user")]
    [Route("{action=index}")]
    public class UserController : Controller
    {
        vlutrading3545Entities db = new vlutrading3545Entities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult register()
        {
            if (Session["userLogged"] != null)
            {
                return RedirectToAction("account_settings", "User");
            }
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var ques = db.security_question.ToList();
                List<SelectListItem> item = new List<SelectListItem>();
                foreach (var i in ques)
                {
                    item.Add(new SelectListItem
                    {
                        Text = i.question,
                        Value = i.id.ToString()
                    });
                }

                ViewBag.question = item;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult register(USERMetadata newUser)
        {
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var ques = db.security_question.ToList();
                List<SelectListItem> item = new List<SelectListItem>();
                foreach (var i in ques)
                {
                    item.Add(new SelectListItem
                    {
                        Text = i.question,
                        Value = i.id.ToString()
                    });
                }

                ViewBag.question = item;

                if (ModelState.IsValid)
                {
                    if (db.users.Any(x => x.email == newUser.email))
                    {
                        ModelState.AddModelError("Email", "Email already exist");
                        return View(newUser);
                    }
                    else if (db.users.Any(x => x.username == newUser.username))
                    {
                        ModelState.AddModelError("Username", "Username already exist");
                        return View(newUser);
                    }
                    else
                    {
                        string ip_login = "";
                        if(Request.UserHostAddress != null)
                        {
                            ip_login = Request.UserHostAddress;
                        }
                        user usr = new user
                        {
                            username = newUser.username,
                            password = hashPwd(newUser.password),
                            email = newUser.email,
                            name = newUser.name,
                            role = 1,
                            id_security_question = newUser.id_security_question,
                            answer_security_question = newUser.answer_security_question,
                            is_active = 1,
                            ip_last_login = ip_login,
                            last_login_date = DateTime.Now,
                            create_by = newUser.username,
                            create_date = DateTime.Now,
                            update_by = newUser.username,
                            update_date = DateTime.Now
                        };
                        try
                        {
                            db.users.Add(usr);
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            ViewBag.DuplicateMessage = "Error occurred while register. Contact Admin for details";
                            return View();
                            throw;
                        }
                        ViewBag.SuccessMessage = "Successful Register";
                        ModelState.Clear();
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult login()
        {
            if(Session["userLogged"] != null)
            {
                return RedirectToAction("account_settings", "User");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(USERMetadata userLogin)
        {
            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {
                if(db.users.Any(x => x.username == userLogin.username))
                {
                    var user = db.users.FirstOrDefault(x => x.username == userLogin.username);
                    if(user.password == hashPwd(userLogin.password))
                    {
                        
                        Session["userLogged"] = user;
                        updateLastLoginTimeAndIp();
                        ViewBag.SuccessMessage = "Successful Logged";
                        ViewBag.LoggedStatus = true;

                    }
                    else
                    {
                        ViewBag.DuplicateMessage = "Login failed!";
                    }
                    
                }
                else
                {
                    ViewBag.DuplicateMessage = "Login failed!";
                }
            }
            if(Session["userLogged"] != null)
            {

            }

            return View();
        }

        [HttpGet]
        public ActionResult logout()
        {
            updateLastLogoutTimeAndIp();
            Session["userLogged"] = null;
            return Redirect(Request.UrlReferrer.ToString());
        }

        [Route("~/user/settings")]
        public ActionResult account_settings()
        {
            if(Session["userLogged"] == null) {
                
                return RedirectToAction("login", "User");
            }
            var user = Session["userLogged"] as TradingVLU.Models.user;

            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {

                ViewBag.user_question = db.users.Join(db.security_question, 
                                            usr => user.id_security_question, 
                                            ques => ques.id, 
                                            (usr, ques) => new {
                                                username = usr.username,
                                                question = ques.question,
                                                answer = usr.answer_security_question
                                            }).ToList();
                var user_detail = db.users.FirstOrDefault(usr => user.id == usr.id);
                ViewBag.user_detail = user_detail;
                
            }
           


            return View();
        }

        [NonAction]
        private void updateLastLoginTimeAndIp()
        {
            var user = Session["userLogged"] as TradingVLU.Models.user;
            if(user != null)
            {
                string ip_login = "default";
                if (Request.UserHostAddress != null)
                {
                    ip_login = Request.UserHostAddress;
                }
                //db.Database.SqlQuery<ObjReturn>("updateLastLoginIpAddress", user_detail.id, ip_login);
                var sql_sp = @"exec updateLastLoginIpAddress {0}, {1}";
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    db.Database.ExecuteSqlCommand(sql_sp,
                                user.id, ip_login);
                }
            }
        }

        [NonAction]
        private void updateLastLogoutTimeAndIp()
        {
            var user = Session["userLogged"] as TradingVLU.Models.user;
            if (user != null)
            {
                string ip_logout = "default";
                if (Request.UserHostAddress != null)
                {
                    ip_logout = Request.UserHostAddress;
                }
                //db.Database.SqlQuery<ObjReturn>("updateLastLoginIpAddress", user_detail.id, ip_login);
                var sql_sp = @"exec updateLastLogoutIpAddress {0}, {1}";
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    db.Database.ExecuteSqlCommand(sql_sp,
                                user.id, ip_logout);
                }
            }


        }

        [NonAction]
        private string hashPwd(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pwd));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            var ques = db.security_question.ToList();
            List<SelectListItem> item = new List<SelectListItem>();
            foreach (var i in ques)
            {
                item.Add(new SelectListItem
                {
                    Text = i.question,
                    Value = i.id.ToString()
                });
            }
            ViewBag.question = item;

            //var user =db.users.FirstOrDefault(x=>x.username)
            //*
            return View(ForgotPassword());


        }
    }
}