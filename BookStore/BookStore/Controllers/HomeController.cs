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
       // private BookStoreDatabaseEntities db = new BookStoreDatabaseEntities();

        public ActionResult Index()
        {
            return View();          
        }       
        public JsonResult getAll()
        {
            using (BookStoreDatabaseEntities dataContext = new BookStoreDatabaseEntities())
            {
                var bookList = (from E in dataContext.Books
                                    join dep in dataContext.Authors on E.AuthorID equals dep.AuthorID
                                    join dsg in dataContext.Countries on E.CountryID equals dsg.CountryID
                                    orderby E.BookID
                                    select new
                                    {
                                        E.Country.Tel_Code,
                                        E.BookID,
                                        E.Title,
                                        E.Price,    
                                        E.Picture
                                    }).ToList();
                var JsonResult = Json(bookList, JsonRequestBehavior.AllowGet);
                JsonResult.MaxJsonLength = int.MaxValue;
                return JsonResult;
            }
        }

    }
}