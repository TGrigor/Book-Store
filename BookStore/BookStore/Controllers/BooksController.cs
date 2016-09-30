using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domein;
using System.IO;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private BookStoreDatabaseEntities db = new BookStoreDatabaseEntities();

        // GET: Books
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


        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            { 
            if (id == null)
            {
                return PartialView("Error404", id.ToString());
            }
            Book book = await db.Books.FindAsync(id);
            book.Price += book.Country.Tel_Code;
            if (book == null)
            {
                return PartialView("Error404", id.ToString());
            }
            return View(book);
            }
            catch
            {
                return PartialView("Error404", id.ToString());

            }
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName");
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");

                return View();
            }
            catch 
            {
                return PartialView("Error404");

            }

        }

        // POST: Books/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,Picture")] Book book , HttpPostedFileBase image1)
        {
            try
            {
                if (image1 != null)
                {
                    FileInfo file = new FileInfo(image1.FileName);
                    var fileName = Path.GetFileName(image1.FileName);
                    string GuIdName = Guid.NewGuid().ToString() + file.Extension;// stugel vor miayn picture tipi filer pahi

                    var path = Path.Combine(Server.MapPath("~/Picture/"), GuIdName);
                    book.Picture = GuIdName;
                    db.Books.Add(book);
                    await db.SaveChangesAsync();
                    image1.SaveAs(path);
                    return RedirectToAction("Index");
                }
                else
                {
                    book.Picture = "noimg.gif";
                    db.Books.Add(book);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch 
            {
                return PartialView("Error404");
            }

        }

        // GET: Books/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("Error404", id.ToString());
                }
                Book book = await db.Books.FindAsync(id);
                if (book == null)
                {
                    return PartialView("Error404", id.ToString());
                }
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", book.CountryID);
                return View(book);
            }
            catch
            {
                return PartialView("Error404", id.ToString());
            }

        }

        // POST: Books/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,Picture")] Book book, HttpPostedFileBase image1)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (image1 != null)
                    {
                        FileInfo file = new FileInfo(image1.FileName);
                        var fileName = Path.GetFileName(image1.FileName);
                        string GuIdName = Guid.NewGuid().ToString() + file.Extension;
                        var path = Path.Combine(Server.MapPath("~/Picture/"), GuIdName);
                        book.Picture = GuIdName;
                        image1.SaveAs(path);
                    }
                    else
                    {
                        using (BookStoreDatabaseEntities db1 = new BookStoreDatabaseEntities())
                        {
                            var img = db1.Books.Find(book.BookID).Picture;
                            book.Picture = img;
                        }

                    }
                    //if (image1!=null)
                    //{
                    //    book.Picture = new byte[image1.ContentLength];
                    //    image1.InputStream.Read(book.Picture, 0, image1.ContentLength);

                    //}
                    //if (image1==null)
                    //{
                    //    using (BookStoreDatabaseEntities db1 = new BookStoreDatabaseEntities())
                    //    {
                    //        var img = db1.Books.Find(book.BookID).Picture;
                    //        book.Picture = img;
                    //    }
                    //}           
                    db.Entry(book).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", book.CountryID);
                return View(book);
            }
            catch
            {
                return PartialView("Error404");

            }

        }

        // GET: Books/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = await db.Books.FindAsync(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);


            }
            catch 
            {

                return PartialView("Error404", id.ToString());
                
            }
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Book book = await db.Books.FindAsync(id);
                db.Books.Remove(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch 
            {
                return PartialView("Error404", id.ToString());

            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
