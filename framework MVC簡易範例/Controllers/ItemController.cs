using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pattern.Website.Models;

namespace Pattern.Website.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index()
        {
            ViewBag.ItemList = "Computer Shop Item List Page";
            
            List<ItemList> IList = new List<ItemList>()
            {
                new ItemList {ID=1, Name="iPhone", Category="Mobile", Price=2393 },
                new ItemList {ID=2, Name="HardDisk", Category="Computer", Price=9399 },
                new ItemList {ID=3, Name="Mouse", Category="Computer", Price=120 },
                new ItemList {ID=4, Name="Samsung Note3", Category="Mobile", Price=9348 }
            };

            return View(IList);
        }
    }
}