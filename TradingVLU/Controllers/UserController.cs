using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                ViewBag.CartUnits = 0;
                ViewBag.Cart = 0;
                ViewBag.CartTotalPrice = 0;
            }
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
                        string ip_login = String.Empty;
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
                            is_active = 0,
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
                        catch (Exception )
                        {
                            ViewBag.DuplicateMessage = "Error occurred while register. Contact Admin for details";
                            return View();
                            throw;
                        }
                        ViewBag.SuccessMessage = "Successful.Your account will be activated in 24h";
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
            if(String.IsNullOrEmpty(userLogin.username) || String.IsNullOrEmpty(userLogin.password))
            {
                ViewBag.DuplicateMessage = "Username or password must not be empyty!";
                return View();
            }
            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {
                if (db.users.Any(x => x.username == userLogin.username))
                {
                    var user = db.users.FirstOrDefault(x => x.username == userLogin.username);
                    if(user.password == hashPwd(userLogin.password))
                    {
                        if(user.is_active != 1)
                        {
                            ViewBag.DuplicateMessage = "Your Account will be Activated within 24h!";
                            return View();
                        }
                        Session["userLogged"] = user;
                        Session["username"] = user.username;
                        Session["userID"] = user.id;
                        updateLastLoginTimeAndIp();
                        ViewBag.SuccessMessage = "Logged-in successfully";
                        ViewBag.LoggedStatus = true;
                        if (user.role == 2 )
                            return RedirectToAction("Index", "User", new { Area= "Admin" });
                        if (user.role == 1002)
                            return RedirectToAction("approve", "ItemManagement");
                        else return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        ViewBag.DuplicateMessage = "Incorrect username or password";
                    }
                }
                else
                {
                    ViewBag.DuplicateMessage = "Incorrect username or password";
                }
            }
            if(Session["userLogged"] != null)
            {

            }

            return View();
        }
        public ActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                int userID = int.Parse(Session["userID"].ToString());
                var user = db.users.FirstOrDefault(x => x.id == userID);
                if (user.password == hashPwd(model.Password) && ModelState.IsValid)
                {
                    user.password = hashPwd(model.NewPassword);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.SuccessMessage = "Successful changed password";
                }
                else
                {
                    ViewBag.DuplicateMessage = "Change Password Fail";
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error occured");
               // throw;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult logout()
        {
            updateLastLogoutTimeAndIp();
            Session["userLogged"] = null;
            ViewBag.Cart = null;
            return RedirectToAction("index", "Home");
        }

        [Route("~/user/settings")]
        public ActionResult account_settings()
        {
            if(Session["userLogged"] == null) {
                
                return RedirectToAction("login", "User");
            }
            else {

            using(vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var user = Session["userLogged"] as TradingVLU.Models.user;
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
                string ip_logout = String.Empty;
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
            return View(ques);


        }
        [HttpPost]
        public void ForgotPassword(string username, int id_security_question,String answer_security_question)
        {
            var user = db.users.FirstOrDefault(x => x.username == username);
            if (user == null)
            {
                Session["erorr_forgot"] = "true";
                Response.Redirect("~/User/ForgotPassword");
            }
            else
            {
                if (user.id_security_question == id_security_question && user.answer_security_question == answer_security_question.Trim())
                {
                    Session["newPassword"] = "true";
                    Session["newpass_user"] = user.id;
                    Response.Redirect("~/User/createnew");
                }
                else
                {
                    Session["erorr_forgot"] = "true";
                    Response.Redirect("~/User/ForgotPassword");
                }
            }
        }
        public ActionResult createnew()
        {
            return View();
        }

        public void UpdatePassword(String Newpassword)
        {
            var userid = Session["newpass_user"];
            var user = db.users.Find(userid);

            user.password = hashPwd(Newpassword);
            db.SaveChanges();
            Session["changeMessage"] = "true";
            Response.Redirect("~/User/createnew");
            Response.Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult updateName(user user)
        {
            int userID = int.Parse(Session["userID"].ToString());
            var xUser = db.users.FirstOrDefault(x => x.id == userID);
            xUser.name = user.name;
            db.Entry(xUser).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.SuccessMessage = "User's name succesfully changed";
            return RedirectToAction("account_settings","User");
        }

    }
}