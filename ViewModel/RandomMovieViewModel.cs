using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pattern.Website.Models;

namespace Pattern.Website.ViewModel
{
    public class RandomMovieViewModel
    {
        public Movie Movie { get; set; }
        public List<Customer> Customers { get; set; }
    }
}