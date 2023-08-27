using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Input;
using Pattern.Website.Models;
using Pattern.Website.ViewModel;

namespace Pattern.Website.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        // GET: /Movies/
        public ActionResult Index()
        {
            return View();
        }

        // GET: /movies/released/2022/08
        [Route("movies/released/{year:regex(\\d{4}):range(2011, 2023)}/{month:regex(\\d{2}):range(1, 12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
            //return View();
        }

        // GET: /Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "TestRandom123321" };

            //ViewData["Movie"] = movie;
            //ViewBag.RandomMovie = movie;

            //var viewResult = new ViewResult();
            //viewResult.ViewData.Model

            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel);
        }

    }

}