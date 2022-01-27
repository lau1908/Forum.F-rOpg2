using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Forum.FørOpg2.Models1;
using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using SignalRChat;

namespace Forum.FørOpg2.Controllers
{

    public class HomeController : Controller
    {
        IHubContext chathub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        public ActionResult LogInPage2()
        {
            return View();

        }

        public ActionResult Index()
        {
            if (Session["uid"] != null)//Tjekker om brugern har en konto med et ID, hvis ja bliver siden vist 
            {
                Debug.WriteLine(Session["uid"]);
                return View();
            }
            else //hvis nej bliver de sendt til loginsiden 
            {
                return RedirectToAction("LogInPage2");
            }

        }


        public ActionResult Friends()
        {
            if (Session["uid"] != null)
            {
                Debug.WriteLine(Session["uid"]);
                return View();
            }
            else
            {
                //return new HttpStatusCodeResult(401);
                return RedirectToAction("LogInPage2");
            }


        }

        public ActionResult Explore()
        {


            if (Session["uid"] != null)
            {
                Debug.WriteLine(Session["uid"]);
                return View();
            }
            else
            {
                return RedirectToAction("LogInPage2");
            }
        }

        public ActionResult Spotify()
        {
            if (Session["uid"] != null)
            {
                Debug.WriteLine(Session["uid"]);
                return View();
            }
            else
            {
                return RedirectToAction("LogInPage2");
            }
        }
        public ActionResult GammingChat()
        {

            if (Session["uid"] != null)
            {
                Debug.WriteLine(Session["uid"]);
                return View();
            }
            else
            {
                //return new HttpStatusCodeResult(401);
                return RedirectToAction("LogInPage2");
            }
        }



        public ActionResult Settings()
        {

            if (Session["uid"] != null)
            {
                Debug.WriteLine(Session["uid"]);
                return View();
            }
            else
            {
                //return new HttpStatusCodeResult(401);
                return RedirectToAction("LogInPage2");
            }
        }

        [HttpPost]
        public ActionResult Chat(string message) //variablen message indeholder content af beskeden 
        {
           if (Session["uid"] != null)
            {
                int uid = Int32.Parse(Session["uid"].ToString());//tildeler variablen "uid" brugerens ID
                ChatDatabaseEntities databasemanager = new ChatDatabaseEntities();//jeg erklærer en entiti af min database til objektet "databaseManager"
                BrugerTB bruger = databasemanager.BrugerTBs.FirstOrDefault(e => e.Bruger_ID == uid);//henter brugerns data fra databasen ved hjælp af brugerens ID

                BeskedTB besked = new BeskedTB() { // Opretter et objekt hvori vi gemmer den sendte besked, ID'et på brugern der sendte beskeden samt forum ID 
                    Besked_content = message, 
                    Bruger_ID = bruger.Bruger_ID, 
                    Forum_ID = 1,//kun gamingchatten virker, da de andre ikke er sat op, gamingchatten har ID'et 1
                };
                databasemanager.BeskedTBs.Add(besked);//Sender objektet til databasen 
                databasemanager.SaveChanges();//gemmer 
                
                return View(chathub.Clients.All.addNewMessageToPage(bruger.Username, message));//retunere et view med brugernavn samt beskeden som outputargument 
            }
            else
            {
                return RedirectToAction("LogInPage2");
            }

        }

        public ActionResult Chat()
        {
          
           
            if (Session["uid"] != null)
            {
                return View();
            }
            else
            {
                //return new HttpStatusCodeResult(401);
                return RedirectToAction("LogInPage2");
            }
           
        }
        public ActionResult LogInPage()
        {
            return View();
        }
        [HttpPost]

        public ActionResult LogInPage(Bruger U)
        {
            string UUsername = U.Username;
            string UEmail = U.Email;
            string UParsword = Crypto.Hash(U.Parsword, "MD5");


            try
            {
                BrugerTB cm = new BrugerTB
                {
                    Username = UUsername,
                    Parsword = UParsword,
                    Email = UEmail
                };
                ChatDatabaseEntities k = new ChatDatabaseEntities();

                var kn = k.BrugerTBs.FirstOrDefault(x => x.Username == UUsername);
                var km = k.BrugerTBs.FirstOrDefault(x => x.Email == UEmail);
                if (kn == null)
                {
                    using (ChatDatabaseEntities e = new ChatDatabaseEntities())
                    {
                        var nyBruger = e.BrugerTBs.Add(cm);
                        e.SaveChanges();
                        Session["uid"] = nyBruger.Bruger_ID;
                        

                        HttpCookie cock = new HttpCookie("username");
                        cock.Value = U.Username;
                        cock.Expires = DateTime.Now.AddMinutes(10);
                        Response.Cookies.Add(cock);
                        ViewBag.Message = "Successfully Registration Done";
                        return RedirectToAction("Index");
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



        [HttpPost]
        public ActionResult LogInPage2(Bruger2 kl)
        {
            using (ChatDatabaseEntities k = new ChatDatabaseEntities())
            {
                var UserMail2 = kl.UserMail;
                var Parsword2 = Crypto.Hash(kl.Parsword, "MD5");

                var user = k.BrugerTBs.FirstOrDefault(z => z.Email == UserMail2 || z.Username == UserMail2);
                var pass = k.BrugerTBs.FirstOrDefault(x => x.Parsword == Parsword2);
                if (user != null && pass != null)
                {
                    ViewBag.Message = "Logget ind";
                    Session["uid"] = user.Bruger_ID;
                    
                    HttpCookie cock = new HttpCookie("username");
                    cock.Value = kl.UserMail;
                    cock.Expires = DateTime.Now.AddMinutes(10);
                    Response.Cookies.Add(cock);
                    return RedirectToAction("Index");
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