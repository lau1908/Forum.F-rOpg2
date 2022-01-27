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

        public ActionResult LogInPage(Bruger U) // en post metode til registering af bruger, så det kan sættes ind i databasen, tager udgangspunkt i klassen Bruger
        {
            string UUsername = U.Username; //en string bliver sat lig med, username som kommer fra klassen bruger, som kommer fra hvad brugeren har indtastet
            string UEmail = U.Email;
            string UParsword = Crypto.Hash(U.Parsword, "MD5"); //hash af kode


            try
            {
                BrugerTB cm = new BrugerTB // der bliver lavet et nyt BrugerTB, hvor de forskellige fra tablet bliver sat lig med, de oplysninger brugeren har indtastet
                {
                    Username = UUsername,
                    Parsword = UParsword,
                    Email = UEmail
                };
                ChatDatabaseEntities k = new ChatDatabaseEntities(); //jeg erklærer en entiti af min database til objektet "k"

                var kn = k.BrugerTBs.FirstOrDefault(x => x.Username == UUsername); //tjekker om der allerede er en bruger med det samme brugernavn
                var km = k.BrugerTBs.FirstOrDefault(x => x.Email == UEmail); //tjekker om der allerede er en bruger med samme email
                if (kn == null && km==null) //hvis en bruger med dette brugernavn ikke findes
                {
                    using (ChatDatabaseEntities e = new ChatDatabaseEntities()) //jeg erklærer en entiti af min database til objektet "e"
                    {
                        var nyBruger = e.BrugerTBs.Add(cm); //der er en variabel nybruger, hvor disse informationer bliver sat ind i databasen
                        e.SaveChanges(); //her bliver det egentlig saved
                        Session["uid"] = nyBruger.Bruger_ID; //laver en session med brugerens id
                        

                        HttpCookie cock = new HttpCookie("username"); //der bliver lavet en cookie som indeholder brugernavnet
                        cock.Value = U.Username; 
                        cock.Expires = DateTime.Now.AddMinutes(10); //den holder i 10 minutter
                        Response.Cookies.Add(cock); 
                        ViewBag.Message = "Successfully Registration Done"; //der bliver sendt en message
                        return RedirectToAction("Index"); //bliver sendt til index siden
                    }
                }
                else if (kn != null) //hvis der allerede findes en bruger med dette brugernavn
                {
                    ViewBag.Message = "Der findes allerde en bruger med dette usernavn"; 
                }
                if (km != null)//hvis der allerede findes en bruger med dette brugernavn
                {
                    ViewBag.Message = "Der findes allerde en bruger med denne Email";
                }


            }
            catch (Exception ex) // egentlig en blok der "fanger" de undtagelser der sker, disse undtagelser er dog noget man allerede "forventer" 
            {
                throw ex;

            }

            return View();
        }



        [HttpPost]
        public ActionResult LogInPage2(Bruger2 kl) //en post metode til login af brugeren, så det kan sættes ind i databasen, tager udgangspunkt i klassen Bruger2
        {
            using (ChatDatabaseEntities k = new ChatDatabaseEntities()) //jeg erklærer en entiti af min database til objektet "k"
            {
                var UserMail2 = kl.UserMail; //usermail inde fra Bruger2 bliver sat lig med variablen
                var Parsword2 = Crypto.Hash(kl.Parsword, "MD5"); // koden bliver hashet

                var user = k.BrugerTBs.FirstOrDefault(z => z.Email == UserMail2 || z.Username == UserMail2); //tjekker om informationen, matcher enten et brugernavn eller email
                var pass = k.BrugerTBs.FirstOrDefault(x => x.Parsword == Parsword2); //tjekker om informationen, matcher en adgangskode i db'en
                if (user != null && pass != null) //hvis det hele matcher
                {
                    ViewBag.Message = "Logget ind"; //bliver sendt en besked ind
                    Session["uid"] = user.Bruger_ID; // der bliver lavet en session, som indeholder brugerens id
                    
                    HttpCookie cock = new HttpCookie("username");//der bliver lavet en cookie som indeholder brugernavnet
                    cock.Value = kl.UserMail;
                    cock.Expires = DateTime.Now.AddMinutes(10); //den holder i 10 minutter
                    Response.Cookies.Add(cock);
                    return RedirectToAction("Index"); //bliver sendt til index siden
                }
                else if (user == null || pass == null) // hvis der ikke er noget der matcher kommer denne besked op
                {
                    ViewBag.Message = "Forkert brugernavn/mail eller kode";

                }
            }
            return View();

        }
    }
}