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
    // added authorization so that user should be logged in to create, edit, delete the movie
    //added after presentation
    [Authorize]
    public class MoviesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Movies
        [AllowAnonymous]
        public ActionResult Index()
        {
            var movies = db.Movies;
            return View(movies.ToList());
        }

        // GET: Movies/Details/5
        [AllowAnonymous]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            Movie model = new Movie();
            model.Name = String.Format("Movie - {0}", DateTime.Now.Ticks);

            ViewBag.Actors = new MultiSelectList(db.Actors.ToList(), "ActorId", "Name", model.Actors.Select(x=>x.ActorId).ToArray());

            return View(model);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,HitMovie,ActorIds")] Movie model, string[] ActorIds)
        {
            if (ModelState.IsValid)
            {
                Movie checkmodel = db.Movies.SingleOrDefault(x => x.Name == model.Name && x.HitMovie == model.HitMovie);

                if (checkmodel == null)
                {
                    //model.MovieId = Guid.NewGuid().ToString();
                    //model.CreateDate = DateTime.Now;
                    //model.EditDate = model.CreateDate;
                    db.Movies.Add(model);
                    db.SaveChanges();

                    if (ActorIds != null)
                    {
                        foreach (string actorId in ActorIds)
                        {
                            MovieActor movieActor = new MovieActor();

                            //movieActor.MovieActorId = Guid.NewGuid().ToString();
                            //movieActor.CreateDate = DateTime.Now;
                            //movieActor.EditDate = movieActor.CreateDate;

                            movieActor.MovieId = model.MovieId;
                            movieActor.ActorId = actorId;
                            model.Actors.Add(movieActor);
                        }
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicated Movie detected.");
                }
            }

            ViewBag.Actors = new MultiSelectList(db.Actors.ToList(), "ActorId", "Name", ActorIds);

            return View(model);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie model = db.Movies.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.Actors = new MultiSelectList(db.Actors.ToList(), "ActorId", "Name", model.Actors.Select(x => x.ActorId).ToArray());

            return View(model);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieId,Name,HitMovie,ActorIds")] Movie model, string[] ActorIds)
        {
            if (ModelState.IsValid)
            {
                Movie tmpmodel = db.Movies.Find(model.MovieId);
                if(tmpmodel != null)
                {
                    Movie checkmodel = db.Movies.SingleOrDefault(
                                        x=>x.Name == model.Name && 
                                        x.HitMovie == model.HitMovie &&
                                        x.MovieId != model.MovieId);

                    if (checkmodel == null)
                    {
                        tmpmodel.Name = model.Name;
                        tmpmodel.HitMovie = model.HitMovie;
                        tmpmodel.EditDate = DateTime.Now;

                        db.Entry(tmpmodel).State = EntityState.Modified;

                        //Items to remove
                        var removeItems = tmpmodel.Actors.Where(x => !ActorIds.Contains(x.ActorId)).ToList();

                        foreach (var removeItem in removeItems)
                        {
                            db.Entry(removeItem).State = EntityState.Deleted;
                        }

                        if (ActorIds != null)
                        {
                            var addedItems = ActorIds.Where(x => !tmpmodel.Actors.Select(y => y.ActorId).Contains(x));

                            //Items to add
                            foreach (string addedItem in addedItems)
                            {
                                MovieActor movieActor = new MovieActor();

                                movieActor.MovieActorId = Guid.NewGuid().ToString();
                                movieActor.CreateDate = DateTime.Now;
                                movieActor.EditDate = movieActor.CreateDate;

                                movieActor.MovieId = tmpmodel.MovieId;
                                movieActor.ActorId = addedItem;
                                db.MovieActors.Add(movieActor);
                            }
                        }

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Duplicated Movie detected.");
                    }
                }
            }

            ViewBag.Actors = new MultiSelectList(db.Actors.ToList(), "ActorId", "Name", ActorIds);

            return View(model);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Movie model = db.Movies.Find(id);

            if(model == null)
            {
                return HttpNotFound();
            }

            foreach (var item in model.Actors.ToList())
            {
                db.MovieActors.Remove(item);
            }

            db.Movies.Remove(model);

            var deleted = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);
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
