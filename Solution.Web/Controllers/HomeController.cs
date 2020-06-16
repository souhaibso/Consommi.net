using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solution.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Product()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Shop()
        {
            ViewBag.Message = "Your shop page.";

            return View();
        }

        public ActionResult ShopingCart()
        {
            ViewBag.Message = "Your shop page.";

            return View();
        }



        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult BlogDetails()
        {
            return View();
        }


        public ActionResult Checkout()
        {
            return View();
        }

    }
}