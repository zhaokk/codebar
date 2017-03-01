using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using roletest.Models;
using Microsoft.AspNet.Identity;
using IdentitySample.Models;

namespace roletest.Controllers
{
    public class TeachersController : Controller
    {
        private Entities db = new Entities();

        // GET: Teachers
        public ActionResult Index()
        {
            var teachers = db.Teachers.Include(t => t.AspNetUser);
            return View(teachers.ToList());
        }
        [Authorize(Roles = "Teacher")]
        public ActionResult ManageSections()
        {
           var userId= User.Identity.GetUserId();
            Teacher teacher = db.Teachers.First(tea => tea.UserId == userId);
            List<Section> listSec = teacher.Sections.ToList();

            return View(listSec);
        }
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateSection()
        {


            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateSection([Bind(Include = "Id,SectionDetail,SectionName")] Section section) {
            var userId = User.Identity.GetUserId();
            Teacher teacher = db.Teachers.First(tea => tea.UserId == userId);
            if (ModelState.IsValid) {
                section.Teacher = teacher;
                db.Sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("ManageSections");
            }

            return View(section);
        }
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateStudent(int? sectionId) {
            RegisterViewModelOfStudentToSection register = new RegisterViewModelOfStudentToSection();
            register.SectionId = (int)sectionId;
            return View();
        }
        [HandleError]
        [Authorize(Roles = "Teacher")]
        public ActionResult StudentManagement(int? id) {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section theSection = db.Sections.First(sec => sec.Id == id);
            if (theSection == null)
            {
                return HttpNotFound();
            }
            

            return View(theSection);
        }
        // GET: Teachers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,School,Address")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", teacher.UserId);
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", teacher.UserId);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,School,Address")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", teacher.UserId);
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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
