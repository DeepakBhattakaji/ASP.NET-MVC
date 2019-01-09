using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Login_Signup.Models;


namespace Login_Signup.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Registration(int id=0)
        {
            tbl_UserInfo userInfo = new tbl_UserInfo();

            return View(userInfo);
        }

        [HttpPost]
        public ActionResult Registration(tbl_UserInfo userInfo)
        {
            userInfo.Password = Crypto.Hash(userInfo.Password);
            userInfo.ConfirmPassword = Crypto.Hash(userInfo.ConfirmPassword);

            using (DBModels  db = new DBModels())
            {
                if (db.tbl_UserInfo.Any(x => x.Email == userInfo.Email))
                {
                    ViewBag.DuplicateMessage = "User Name Already Exists.";
                    return View("Registration", userInfo);
                }
                else
                {
                    db.tbl_UserInfo.Add(userInfo);
                    db.SaveChanges();
                }
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Success!";
                return View("Registration", new tbl_UserInfo());
        }

        //LOGIN
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //LOGIN POST


            [HttpPost]
            [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login)
        {
           // string message = "";
            using (DBModels model = new DBModels())
            {
                var v = model.tbl_UserInfo.Where(a => a.Email == login.EmailId).FirstOrDefault();
                if(v!=null)
                {
                    //Hash Matching
                    if(string.Compare(Crypto.Hash(login.Password),v.Password)==0)
                    {
                        //  message = "Login Success!";
                        ViewData["Success"] = "Login Success!";
                    }

                    else
                    {
                      //  message = "Invalid Credential Provided!";
                        ViewData["Error"] = "Invalid UserName and Password!";

                    }
                }
                else
                {
                   // message = "Invalid Credential Provided!";
                    ViewData["Error"] = "Invalid UserName and Password";

                }
            }

           // ViewBag.Message = message;
            return View();

        }
    }
}