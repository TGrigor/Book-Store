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
        public async Task<ActionResult> Index(string searchString)
        {
            
            var books = db.Books.Include(b => b.Author).Include(b => b.Country);

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }
            return View(await books.ToListAsync());
        }

        [HttpPost]
        public string Index(FormCollection fc, string searchString)
        {
            return "<h3> From [HttpPost]Index: " + searchString + "</h3>";
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
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

        [Authorize]
        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,Picture")] Book book , HttpPostedFileBase image1)
        {
            //nayel{
            if (image1.ContentLength > 0)
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
            /////}

            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", book.CountryID);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", book.CountryID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,Picture")] Book book, HttpPostedFileBase image1)
        {
            if (ModelState.IsValid)
            {
                if (image1!=null)
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

        // GET: Books/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Book book = await db.Books.FindAsync(id);
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
