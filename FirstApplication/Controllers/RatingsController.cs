//Ratings added after the presentation
using FirstApplication.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace FirstApplication.Controllers
{
    public class RatingsController : Controller
    {
        DataContext db = new DataContext();

        // GET: Ratings
        public ActionResult Index()
        {
            return View();
        }

        public Rating SetRating(string movieId, decimal rank = 0)
        {
            Rating rating = new Rating();
            rating.Rank = rank;
            rating.MovieId = movieId;
            rating.UserId = User.Identity.GetUserId();

            db.Ratings.Add(rating);

            try
            {
                db.SaveChanges();
            }
            

            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            rating = db.Ratings
                        .Include(x => x.Movie)
                        .Include(x => x.Movie.Ratings)
                        .Include(x => x.User)
                        .SingleOrDefault(x => x.RatingId == rating.RatingId);

            return (rating);
            //return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        public PartialViewResult RatingsControl(string movieId)
        {
            Movie model = db.Movies.Find(movieId);

            return PartialView("_RatingsControl", model);
        }
    }
}