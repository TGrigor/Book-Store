using Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        //private BookStoreDatabaseEntities db = new BookStoreDatabaseEntities();

        public ActionResult Index(string searchBy, string search)
        {
            //    if (searchBy=="BookName")
            //    {
            //         return View(db.Books.Where(x => x.Title ==search || search == null).ToList());
            //    }
            //    else 
            //         return View(db.Authors.Where(x => x.AuthorName ==search).ToList());
            return View();
        }


    }
}