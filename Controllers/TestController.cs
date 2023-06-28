using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pattern.Website.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if(!pageIndex.HasValue)
                pageIndex = 1;
            if (String.IsNullOrWhiteSpace(sortBy)) //如果字串為空或空格就傳遞排序變數
                sortBy = "Name"; //初始化sortBy為Name

            //return View();
            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }

        public ActionResult Random()
        {
            //var movie = new Movie() { Name = "TESTTEST" };

            //return View(movie);
            return Content("Hello Test / Random");
        }

        public ActionResult Edit(int id)
        {
            //return View();
            return Content("id = " + id);
        }
    }
}