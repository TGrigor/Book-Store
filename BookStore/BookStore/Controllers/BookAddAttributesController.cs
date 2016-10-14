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

namespace BookStore.Controllers
{
    public class BookAddAttributesController : Controller
    {
        private BookStoreDatabaseEntities db = new BookStoreDatabaseEntities();

        // GET: BookAddAttributes
        public async Task<ActionResult> Index()
        {
            var bookAddAttributes = db.BookAddAttributes.Include(b => b.Book).Include(b => b.ExtraAttribute).Include(b => b.Ganre1);
            return View(await bookAddAttributes.ToListAsync());
        }

        // GET: BookAddAttributes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookAddAttribute bookAddAttribute = await db.BookAddAttributes.FindAsync(id);
            if (bookAddAttribute == null)
            {
                return HttpNotFound();
            }
            return View(bookAddAttribute);
        }

        // GET: BookAddAttributes/Create
        public ActionResult Create()
        {
            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title");
            ViewBag.AttributeID = new SelectList(db.ExtraAttributes, "AttributeID", "Name");
            ViewBag.GanreID = new SelectList(db.Ganres, "GanreID", "GanreName");
            return View();
        }

        // POST: BookAddAttributes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookAddAttributesID,BookID,AttributeID,ValueTypeText,ValueTypeDate,Ganre,GanreID")] BookAddAttribute bookAddAttribute)
        {
            if (ModelState.IsValid)
            {
                db.BookAddAttributes.Add(bookAddAttribute);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", bookAddAttribute.BookID);
            ViewBag.AttributeID = new SelectList(db.ExtraAttributes, "AttributeID", "Name", bookAddAttribute.AttributeID);
            ViewBag.GanreID = new SelectList(db.Ganres, "GanreID", "GanreName", bookAddAttribute.GanreID);
            return View(bookAddAttribute);
        }

        // GET: BookAddAttributes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookAddAttribute bookAddAttribute = await db.BookAddAttributes.FindAsync(id);
            if (bookAddAttribute == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", bookAddAttribute.BookID);
            ViewBag.AttributeID = new SelectList(db.ExtraAttributes, "AttributeID", "Name", bookAddAttribute.AttributeID);
            ViewBag.GanreID = new SelectList(db.Ganres, "GanreID", "GanreName", bookAddAttribute.GanreID);
            return View(bookAddAttribute);
        }

        // POST: BookAddAttributes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookAddAttributesID,BookID,AttributeID,ValueTypeText,ValueTypeDate,Ganre,GanreID")] BookAddAttribute bookAddAttribute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookAddAttribute).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", bookAddAttribute.BookID);
            ViewBag.AttributeID = new SelectList(db.ExtraAttributes, "AttributeID", "Name", bookAddAttribute.AttributeID);
            ViewBag.GanreID = new SelectList(db.Ganres, "GanreID", "GanreName", bookAddAttribute.GanreID);
            return View(bookAddAttribute);
        }

        // GET: BookAddAttributes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookAddAttribute bookAddAttribute = await db.BookAddAttributes.FindAsync(id);
            if (bookAddAttribute == null)
            {
                return HttpNotFound();
            }
            return View(bookAddAttribute);
        }

        // POST: BookAddAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BookAddAttribute bookAddAttribute = await db.BookAddAttributes.FindAsync(id);
            db.BookAddAttributes.Remove(bookAddAttribute);
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
