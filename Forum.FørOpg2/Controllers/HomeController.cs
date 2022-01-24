using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Forum.FørOpg2.Models1;
using System.Data.Entity;
using System.Diagnostics;

namespace Forum.FørOpg2.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Friends()
        {
            
            return View();
        }

        public ActionResult Explore()
        {
            

            return View();
        }

        public ActionResult Spotify()
        {


            return View();
        } public ActionResult GammingChat()
        {


            return View();
        }



        public ActionResult Settings()
        {


            return View();
        }
       
        public ActionResult Chat()
        {
            if (Session["username"] != null)
            {
                Debug.WriteLine(Session["username"]);
                return View();
            } else
            {
                return new HttpStatusCodeResult(401);
            }
        }
        public ActionResult LogInPage()
        {
            return View();
        }
        [HttpPost]

        public ActionResult LogInPage(Bruger U)
        {
            string UUsername = Crypto.Hash(U.Username.ToLower(), "MD5");
            string UEmail = U.Email.ToLower();
            string UParsword = Crypto.Hash(U.Parsword.ToLower(), "MD5");


            try
            {
                BrugerTB cm = new BrugerTB
                {
                    Username = UUsername,
                    Parsword = UParsword,
                    Email = UEmail
                };
                ChatDatabaseEntities k = new ChatDatabaseEntities();

                var kn = k.BrugerTBs.FirstOrDefault(x => x.Username.ToLower() == UUsername);
                var km = k.BrugerTBs.FirstOrDefault(x => x.Email.ToLower() == UEmail);
                if (kn == null)
                {
                    using (ChatDatabaseEntities e = new ChatDatabaseEntities())
                    {
                        e.BrugerTBs.Add(cm);
                        e.SaveChanges();
                        Session["username"] = U.Username;

                        HttpCookie cock = new HttpCookie("username");
                        cock.Value = U.Username;
                        cock.Expires = DateTime.Now.AddMinutes(10);
                        Response.Cookies.Add(cock);
                        ViewBag.Message = "Successfully Registration Done";
                    }
                }
                else if (kn != null)
                {
                    ViewBag.Message = "Der findes allerde en bruger med dette usernavn";
                }
                if (km != null)
                {
                    ViewBag.Message = "Der findes allerde en bruger med denne Email";
                }


            }
            catch (Exception ex)
            {
                throw ex;

            }

            return View();
        }


        public ActionResult LogInPage2()
        {
            return View();

        }
        [HttpPost]
        public ActionResult LogInPage2(Bruger2 kl)
        {
            using (ChatDatabaseEntities k = new ChatDatabaseEntities())
            {
                var UserMail2 = Crypto.Hash(kl.UserMail.ToLower(), "MD5");
                var Parsword2 = Crypto.Hash(kl.Parsword, "MD5");

                var user = k.BrugerTBs.FirstOrDefault(z => z.Email.ToLower() == UserMail2 || z.Username.ToLower() == UserMail2);
                var pass = k.BrugerTBs.FirstOrDefault(x => x.Parsword == Parsword2);
                if (user != null && pass != null)
                {
                    ViewBag.Message = "Logget ind";
                    Session["username"] = kl.UserMail;

                    HttpCookie cock = new HttpCookie("username");
                    cock.Value = kl.UserMail;
                    cock.Expires = DateTime.Now.AddMinutes(10);
                    Response.Cookies.Add(cock);
                }
                else if (user == null || pass == null)
                {
                    ViewBag.Message = "Forkert brugernavn/mail eller kode";

                }
            }
            return View();

        }
    }
}