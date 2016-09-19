using Domein;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
         private BookStoreDatabaseEntities db = new BookStoreDatabaseEntities();

        public async Task<ActionResult> Index()
        {
            var books = db.Books.Include(b => b.Author).Include(b => b.Country);
            return View(await books.ToListAsync());
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
                                        E.Title,
                                        E.Price,
                                        E.BookID,
                                        E.Picture,
                                    }).ToList();
                var JsonResult = Json(bookList, JsonRequestBehavior.AllowGet);
                JsonResult.MaxJsonLength = int.MaxValue;
                return JsonResult;
            }
        }

    }
}