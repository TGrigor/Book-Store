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

        [Authorize]
        // GET: BookAddAttributes
        public async Task<ActionResult> Index()
        {
            try
            {
                var bookAddAttributes = db.BookAddAttributes.Include(b => b.Book).Include(b => b.ExtraAttribute).Include(b => b.Ganre1);
                return View(await bookAddAttributes.ToListAsync());
            }
            catch 
            {
                return PartialView("Error404");
            }

        }

        [Authorize]
        // GET: BookAddAttributes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("Error404");
                }
                BookAddAttribute bookAddAttribute = await db.BookAddAttributes.FindAsync(id);
                if (bookAddAttribute == null)
                {
                    return PartialView("Error404");
                }
                return View(bookAddAttribute);
            }
            catch
            {
                return PartialView("Error404");
            }

        }

        [Authorize]
        // GET: BookAddAttributes/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.BookID = new SelectList(db.Books, "BookID", "Title");
                ViewBag.AttributeID = new SelectList(db.ExtraAttributes, "AttributeID", "Name");
                ViewBag.GanreID = new SelectList(db.Ganres, "GanreID", "GanreName");
                return View();
            }
            catch
            {
                return PartialView("Error404");
            }

        }

        // POST: BookAddAttributes/Create        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookAddAttributesID,BookID,AttributeID,ValueTypeText,ValueTypeDate,Ganre,GanreID")] BookAddAttribute bookAddAttribute)
        {
            try
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
            catch 
            {
                return PartialView("Error404");
            }

        }

        [Authorize]
        // GET: BookAddAttributes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("Error404");
                }
                BookAddAttribute bookAddAttribute = await db.BookAddAttributes.FindAsync(id);
                if (bookAddAttribute == null)
                {
                    return PartialView("Error404");
                }
                ViewBag.BookID = new SelectList(db.Books, "BookID", "Title", bookAddAttribute.BookID);
                ViewBag.AttributeID = new SelectList(db.ExtraAttributes, "AttributeID", "Name", bookAddAttribute.AttributeID);
                ViewBag.GanreID = new SelectList(db.Ganres, "GanreID", "GanreName", bookAddAttribute.GanreID);
                return View(bookAddAttribute);
            }
            catch
            {
                return PartialView("Error404");
            }

        }

        // POST: BookAddAttributes/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookAddAttributesID,BookID,AttributeID,ValueTypeText,ValueTypeDate,Ganre,GanreID")] BookAddAttribute bookAddAttribute)
        {
            try
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
            catch
            {
                return PartialView("Error404");
            }

        }

        [Authorize]
        // GET: BookAddAttributes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            { 
            if (id == null)
            {
                    return PartialView("Error404");
            }
            BookAddAttribute bookAddAttribute = await db.BookAddAttributes.FindAsync(id);
            if (bookAddAttribute == null)
            {
                return PartialView("Error404");
            }
                return View(bookAddAttribute);
            }
            catch
            {
                return PartialView("Error404");
            }
        }

        [Authorize]
        // POST: BookAddAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            { 
            BookAddAttribute bookAddAttribute = await db.BookAddAttributes.FindAsync(id);
            db.BookAddAttributes.Remove(bookAddAttribute);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
            }
            catch
            {
                return PartialView("Error404");
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
