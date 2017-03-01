using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using roletest.Models;
using System.Drawing;
using System.IO;
using Microsoft.AspNet.Identity;

namespace roletest.Controllers
{
    public class StudentsController : Controller
    {
        private Entities db = new Entities();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.AspNetUser);
            return View(students.ToList());
        }
        public ActionResult IndexPartial()
        {
            var students = db.Students.Include(s => s.AspNetUser);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Gender,Age")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", student.Id);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", student.Id);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Gender,Age")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", student.Id);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        public ActionResult LifeCycle()
        {

            return View();
        }
        public ActionResult LifeCycleCode() {

            return View();
        }
        public ActionResult SaveLifeCycle(string image) {
            LifeCycle newLifeCycle = new LifeCycle();
            int num= db.LifeCycles.Count();
            var id = num * 100;
            newLifeCycle.Id = id;
            var stringId = id.ToString();
            newLifeCycle.StudentId = User.Identity.GetUserId();
            newLifeCycle.directory = "../Content/lifecycle/image/" + stringId + ".png";
            newLifeCycle.name = "sample";
            db.LifeCycles.Add(newLifeCycle);
            db.SaveChanges();
            string subString = image.Substring(22, image.Length-22);
            byte[] bytes = Convert.FromBase64String(subString);

            Image theImage;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                theImage = Image.FromStream(ms);
            }
            string path = @"~/Content/lifecycle/image/" + stringId + ".png";
            theImage.Save(Server.MapPath(path), System.Drawing.Imaging.ImageFormat.Png);
            // var i2 = new Bitmap(theImage);

            // i2.Save(Server.MapPath(path), System.Drawing.Imaging.ImageFormat.Png);
            ViewBag.dir = ".."+path.Substring(1,path.Length-1);
            return View();
        }
        [Authorize(Roles = "Student")]
        public ActionResult Gallery() {
            string userid = User.Identity.GetUserId();
            Student student = db.Students.First(stu => stu.Id == userid);
            return View(student);
        }
        public ActionResult LifeCycleNav() {


            return View();
        }
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
