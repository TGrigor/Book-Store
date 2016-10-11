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
    public class AttributesController : Controller
    {
        private BookStoreDatabaseEntities db = new BookStoreDatabaseEntities();

        // GET: Attributes
        public async Task<ActionResult> Index()
        {
            var extraAttributes = db.ExtraAttributes.Include(e => e.AttributeType);
            return View(await extraAttributes.ToListAsync());
        }

        // GET: Attributes/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ExtraAttribute extraAttribute = await db.ExtraAttributes.FindAsync(id);
        //    if (extraAttribute == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(extraAttribute);
        //}

        // GET: Attributes/Create
        public ActionResult Create()
        {
            ViewBag.AttributeTypeID = new SelectList(db.AttributeTypes, "AttributeTypeID", "Name");
            return View();
        }

        // POST: Attributes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AttributeID,Name,AttributeTypeID")] ExtraAttribute extraAttribute)
        {
            if (ModelState.IsValid)
            {
                db.ExtraAttributes.Add(extraAttribute);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AttributeTypeID = new SelectList(db.AttributeTypes, "AttributeTypeID", "Name", extraAttribute.AttributeTypeID);
            return View(extraAttribute);
        }

        // GET: Attributes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtraAttribute extraAttribute = await db.ExtraAttributes.FindAsync(id);
            if (extraAttribute == null)
            {
                return HttpNotFound();
            }
            ViewBag.AttributeTypeID = new SelectList(db.AttributeTypes, "AttributeTypeID", "Name", extraAttribute.AttributeTypeID);
            return View(extraAttribute);
        }

        // POST: Attributes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AttributeID,Name,AttributeTypeID")] ExtraAttribute extraAttribute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(extraAttribute).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AttributeTypeID = new SelectList(db.AttributeTypes, "AttributeTypeID", "Name", extraAttribute.AttributeTypeID);
            return View(extraAttribute);
        }

        // GET: Attributes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtraAttribute extraAttribute = await db.ExtraAttributes.FindAsync(id);
            if (extraAttribute == null)
            {
                return HttpNotFound();
            }
            return View(extraAttribute);
        }

        // POST: Attributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExtraAttribute extraAttribute = await db.ExtraAttributes.FindAsync(id);
            db.ExtraAttributes.Remove(extraAttribute);
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
