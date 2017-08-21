using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstApplication.Models;

namespace FirstApplication.Controllers
{
    public class MovieActorsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: MovieActors
        public ActionResult Index()
        {
            var movieActors = db.MovieActors.Include(g => g.Movie).Include(g => g.Actor);
            return View(movieActors.ToList());
        }

        // GET: MovieActors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieActor movieActor = db.MovieActors.Find(id);
            if (movieActor == null)
            {
                return HttpNotFound();
            }
            return View(movieActor);
        }

        // GET: MovieActors/Create
        public ActionResult Create()
        {
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Name");
            ViewBag.ActorId = new SelectList(db.Actors, "ActorId", "Name");
            return View();
        }

        // POST: MovieActors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieId,ActorId")] MovieActor model)
        {
            if (ModelState.IsValid)
            {
                MovieActor tmpmovieactor = db.MovieActors
                                    .SingleOrDefault(x => x.MovieId == model.MovieId && x.ActorId == model.ActorId);
                if(tmpmovieactor == null)
                {
                    model.MovieActorId = Guid.NewGuid().ToString();
                    model.CreateDate = DateTime.Now;
                    model.EditDate = model.CreateDate;

                    db.MovieActors.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate key found!");
                }
            }

            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Name", model.MovieId);
            ViewBag.ActorId = new SelectList(db.Actors, "ActorId", "Name", model.ActorId);
            return View(model);
        }

        // GET: MovieActors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieActor movieActor = db.MovieActors.Find(id);
            if (movieActor == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Name", movieActor.MovieId);
            ViewBag.ActorId = new SelectList(db.Actors, "ActorId", "Name", movieActor.ActorId);
            return View(movieActor);
        }

        // POST: MovieActors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieActorId,CreateDate,EditDate,MovieId,ActorId")] MovieActor movieActor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movieActor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Name", movieActor.MovieId);
            ViewBag.ActorId = new SelectList(db.Actors, "ActorId", "Name", movieActor.ActorId);
            return View(movieActor);
        }

        // GET: MovieActors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieActor movieActor = db.MovieActors.Find(id);
            if (movieActor == null)
            {
                return HttpNotFound();
            }
            return View(movieActor);
        }

        // POST: MovieActors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MovieActor movieActor = db.MovieActors.Find(id);
            db.MovieActors.Remove(movieActor);
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
