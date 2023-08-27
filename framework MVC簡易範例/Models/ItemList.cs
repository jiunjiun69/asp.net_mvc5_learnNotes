using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pattern.Website.Models
{
    public class ItemList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}