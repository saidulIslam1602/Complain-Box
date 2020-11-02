//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Configuration;
using System.Data.Entity.Migrations.Infrastructure;
using System.Net.Configuration;
using System.Net.Mail;
using FinalYearProjectDYC.Models.ComplainModel;
using FinalYearProjectDYC.Models.LoginModel;
using System.Web.Mvc;
using System.Web.Security;
using FinalYearProjectDYC.Models;


namespace FinalYearProjectDYC.Controllers
{
    public class ComplainController : Controller
    {
        // GET: Complain
        DatabaseDropYourComplainEntities db = new DatabaseDropYourComplainEntities();
     

        public bool ComplainIsExist(AddComplainInDatabaseTable addComplain)
        {

            int i = db.AddComplainInDatabaseTables.Where(c => c.ComplainCategory == addComplain.ComplainCategory && c.ComplainaArea == addComplain.ComplainaArea && c.RoadNumber == addComplain.RoadNumber && c.GiveStatus !="Done").Count();

            if (i > 0)
            {

                return true;
            }



            return false;


        }

        //public ActionResult Exist(AddComplainInDatabaseTable addComplain)
        //{
        //    //string i = db.AddComplainInDatabaseTables.Where(c => c.ComplainCategory == addComplain.ComplainCategory && c.ComplainaArea == addComplain.ComplainaArea && c.RoadNumber == addComplain.RoadNumber).Count();



        //    // return View(db.AddComplainInDatabaseTables.Where(c => c.ComplainCategory == addComplain.ComplainCategory && c.ComplainaArea == addComplain.ComplainaArea && c.RoadNumber == addComplain.RoadNumber).ToList());

        //    //var category = from s in db.AddComplainInDatabaseTables select s;
        //    ////var category = from s in _dbContext.CategoryDatabaSet select s;

        //    //    category = category.Where(s => s.ComplainCategory == ) ;
        //    var courses = db.AddComplainInDatabaseTables.Where(c => c.ComplainCategory == addComplain.ComplainCategory );
        //    return View(courses.ToList());
           






           

        //}





        public static string EmailId;


        public ActionResult AddNewComplain()
        {

            var list = new List<string> { "Main Hall Missing", "Draingae", "Road Damage", "Dustbin Problem" };
            ViewBag.list = list;
            var ComplainArea = new List<string> { "Mohammadpur", "MirPur-1", "Dhamondli" };
            ViewBag.ComplainArea = ComplainArea;

            return View();
        }

        [HttpPost]
        public ActionResult AddNewComplain(AddComplainInDatabaseTable d, HttpPostedFileBase imgfile)
        {
            var list = new List<string> { "Main Hall Missing", "Draingae", "Road Damage", "Dustbin Problem" };
            ViewBag.list = list;
            var ComplainArea = new List<string> { "Mohammadpur", "MirPur-1", "Dhamondli" };
            ViewBag.ComplainArea = ComplainArea;

            // ViewBag.list = list;


            AddComplainInDatabaseTable di = new AddComplainInDatabaseTable();
            string path = uploadimage(imgfile);
            if (ComplainIsExist(d))
            {
                ViewBag.msg = "Complain is exist";
                return View();
            }

            if (path.Equals("-1"))
            {

            }
            else
            {
                di.Id = d.Id;
                di.ComplainCategory = d.ComplainCategory;
                di.ComplainaArea = d.ComplainaArea;
                di.RoadNumber = d.RoadNumber;
                di.ComplainDeatils = d.ComplainDeatils;
                di.Photo = path;
                //di.Photo = d.path;
                di.ComplainerName = d.ComplainerName;
                d.GiveStatus = "Pending";
                di.GiveStatus = d.GiveStatus;
                di.Email = d.Email;
                di.ContractNumber = d.ContractNumber;
                // db.AddComplains.Add(di);
                db.AddComplainInDatabaseTables.Add(di);
                EmailId = di.Email;
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.ComplainId = di.Id;
                ViewBag.msg2 = "Your Complain Id Is Given Below";
                ViewBag.msg = "Complain Submited";

                //string sendEmail = ConfigurationManager.AppSettings["SendEmail"];

                //SendEmail(di.Id.ToString());


            }



            return View();
        }


        public string uploadimage(HttpPostedFileBase file)
        {

            Random r = new Random();

            string path = "-1";

            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {

                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {

                    try
                    {



                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));

                        file.SaveAs(path);

                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);



                        //    ViewBag.Message = "File uploaded successfully";

                    }

                    catch (Exception ex)
                    {

                        path = "-1";

                    }

                }

                else
                {

                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");

                }

            }



            else
            {

                Response.Write("<script>alert('Please select a file'); </script>");

                path = "-1";

            }

            return path;

        }

        public ActionResult ComplainList()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("AdminLogin", "Complain");
            }
            return View(db.AddComplainInDatabaseTables.ToList());
        }


        public ActionResult UploadStatus(int? Id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("AdminLogin", "Complain");
            }
            var list = new List<string> { "Pending", "Done" };
            ViewBag.list = list;
            var result = db.AddComplainInDatabaseTables.Find(Id);
            return View(result);
        }

        [HttpPost]
        public ActionResult UploadStatus(AddComplainInDatabaseTable edit)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("AdminLogin", "Complain");
            }
            if (ModelState.IsValid)
            {
                db.Entry(edit).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                     ViewBag.msg = "Update Work Status";
                    return RedirectToAction("Report", "Complain");
                }
            }

            return View();


        }

        public ActionResult Report()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("AdminLogin", "Complain");
            }
            return View(db.AddComplainInDatabaseTables.ToList());
        }
        //[Authorize(Roles = "Admin")]


        
        public ActionResult CheckComplaiStatus(string search)
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("ComplainerLogin", "Complain");
            }
            int i = Convert.ToInt16(search);
            return View(db.AddComplainInDatabaseTables.Where(x => x.Id == i).ToList());


            //  }



        }

        public ActionResult Details(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("AdminLogin", "Complain");
            }
            AddComplainInDatabaseTable d = new AddComplainInDatabaseTable();
            d = db.AddComplainInDatabaseTables.Where(x => x.Id == id).FirstOrDefault();


            return View(d);
        }

        public ActionResult RegistrationOrLoginPageLink()
        {
            return View();

        }

        public ActionResult ComplainerRegistraion()
        {
            return View();

        }

        [HttpPost]
        public ActionResult ComplainerRegistraion(Registration g)
        {
            DatabaseDropYourComplainEntities3 db = new DatabaseDropYourComplainEntities3();
            db.Registrations.Add(g);
            var st = db.SaveChanges();
            if (st > 0)
            {
                ViewBag.msg = "Registration Done Successfully";
                return RedirectToAction("CheckComplaiStatus");

            }
            else
            {
                ViewBag.msg = "Invailed! Email or Password";
            }
            ModelState.Clear();
            return View();
        }


        public ActionResult ComplainerLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ComplainerLogin(Registration lg )
        {
            if (ModelState.IsValid)
            {
                using (DatabaseDropYourComplainEntities3 ue = new DatabaseDropYourComplainEntities3())
                {
                    var log = ue.Registrations.Where(a => a.UserName.Equals(lg.UserName) && a.Passwords.Equals(lg.Passwords)).FirstOrDefault();
                    if (log != null)
                    {
                        Session["UserName"] = log.UserName;
                        return RedirectToAction("CheckComplaiStatus", "Complain");
                    }

                    else
                    {
                        Response.Write("<script> alert('Invaild password')</script>");
                    }
                }
            }
            return View();
           
        }
        public ActionResult ComplainerLogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("AddNewComplain", "Complain");
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult AdminLogin(Login lg)
        //{
        //    //Login lg = new Login();
        //    if (ModelState.IsValid)
        //    {
        //        using (DatabaseDropYourComplainEntities2 ue = new DatabaseDropYourComplainEntities2())
        //        {
        //            var log = ue.Logins.Where(a => a.UserName.Equals(lg.UserName) && a.Password.Equals(lg.Password)).FirstOrDefault();
        //            if (log != null)
        //            {
        //                Session["UserName"] = log.UserName;
        //                return RedirectToAction("ComplainList", "Complain");
        //            }

        //            else
        //            {
        //                Response.Write("<script> alert('Invaild password')</script>");
        //            }
        //        }
        //    }
        //    return View();
        //}

        [HttpPost]
        public ActionResult AdminLogin(Login lg)
        {
            //Login lg = new Login();
            if (ModelState.IsValid)
            {
                using (DatabaseDropYourComplainEntities2 ue = new DatabaseDropYourComplainEntities2())
                {
                    var log = ue.Logins.Where(a => a.UserName.Equals(lg.UserName) && a.Password.Equals(lg.Password)).FirstOrDefault();
                    if (log != null)
                    {
                        Session["UserName"] = log.UserName;
                        return RedirectToAction("ComplainList", "Complain");
                    }

                    else
                    {
                        Response.Write("<script> alert('Invaild password')</script>");
                    }
                }
            }
            return View();
        }


        //public static void SendEmail(string emailbody)
        //{
        //    // Specify the from and to email address
        //    // AddComplainInDatabaseTable di;
        //    //  Database1Entities1 db = new Database1Entities1();


        //    MailMessage mailMessage = new MailMessage("prantoshon14@gmail.com", EmailId);
        //    // Specify the email body
        //    mailMessage.Body = emailbody;
        //    // Specify the email Subject
        //    mailMessage.Subject = "Your Complain Id ";

        //    // Specify the SMTP server name and post number
        //    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        //    //SmtpClient smtpClient = new SmtpClient();
        //    // Specify your gmail address and password
        //    smtpClient.Credentials = new System.Net.NetworkCredential()
        //    {
        //        UserName = "prantoshon14@gmail.com",
        //        //Password = ""
        //    };
        //    // Gmail works on SSL, so set this property to true
        //    smtpClient.EnableSsl = true;
        //    // Finall send the email message using Send() method
        //    smtpClient.Send(mailMessage);

        //    // string sendEmail = ConfigurationManager.AppSettings["SendEmail"];

        //}

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("AddNewComplain", "Complain");
        }


    }
}