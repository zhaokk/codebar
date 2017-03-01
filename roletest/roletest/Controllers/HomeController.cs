using System.Web.Mvc;
using IdentitySample.Models;
using roletest.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Web;
using System.Net.Mail;

namespace IdentitySample.Controllers

{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Learn() {
            return View();
        }
        public ActionResult Index(bool? noti)
        {
        if (noti != null) {  AspNetUser theUser= db.AspNetUsers.Find(User.Identity.GetUserId());
            if (theUser.newUser == true) {
                theUser.newUser = false;
                db.Entry(theUser).State = EntityState.Modified;
                db.SaveChanges();
               
                ViewBag.noti = 2;
           
            }else
                if (noti == true)
                {
                    ViewBag.noti = 1;
                }
            }
           
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(string theName, string theEmail,string theMessage)
        {
           
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            System.Net.NetworkCredential myCredential = new System.Net.NetworkCredential("username", "password");
            smtpClient.Credentials = myCredential;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(theEmail, "codebar:"+ theName);
            mail.To.Add(new MailAddress("994979136kk@gmail.com"));
            mail.Body = theMessage;
           // smtpClient.Send(mail);
            ViewBag.success = true;

            return View();
        }
    }
}
