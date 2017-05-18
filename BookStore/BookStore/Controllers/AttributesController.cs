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
        [Authorize]
        public async Task<ActionResult> Index()
        {
            try
            { 
            var extraAttributes = db.ExtraAttributes.Include(e => e.AttributeType);
            return View(await extraAttributes.ToListAsync());
            }
            catch
            {
                return PartialView("Error404");
            }
        }

        [Authorize]
        // GET: Attributes/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.AttributeTypeID = new SelectList(db.AttributeTypes, "AttributeTypeID", "Name");
                return View();
            }
            catch 
            {
                return PartialView("Error404");
            }

        }

        // POST: Attributes/Create       
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AttributeID,Name,AttributeTypeID")] ExtraAttribute extraAttribute)
        {
            try
            { 
            if (ModelState.IsValid)
            {
                db.ExtraAttributes.Add(extraAttribute);
                await db.SaveChangesAsync();
                return RedirectToAction("Create", "BookAddAttributes");

            }

            ViewBag.AttributeTypeID = new SelectList(db.AttributeTypes, "AttributeTypeID", "Name", extraAttribute.AttributeTypeID);
            return RedirectToAction("Create","BookAddAttributes");
            }
            catch
            {
                return PartialView("Error404");
            }
        }

        [Authorize]

        // GET: Attributes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            { 
            if (id == null)
            {
                    return PartialView("Error404");
            }
                ExtraAttribute extraAttribute = await db.ExtraAttributes.FindAsync(id);
            if (extraAttribute == null)
            {
                    return PartialView("Error404");
            }
                ViewBag.AttributeTypeID = new SelectList(db.AttributeTypes, "AttributeTypeID", "Name", extraAttribute.AttributeTypeID);
            return View(extraAttribute);
            }
            catch
            {
                return PartialView("Error404");
            }       
        }

        // POST: Attributes/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AttributeID,Name,AttributeTypeID")] ExtraAttribute extraAttribute)
        {
            try
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
            catch
            {
                return PartialView("Error404");
            }
        }

        [Authorize]
        // GET: Attributes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                return PartialView("Error404");
                
                }
                ExtraAttribute extraAttribute = await db.ExtraAttributes.FindAsync(id);
                if (extraAttribute == null)
                {
                return PartialView("Error404");
                   
                }
                return View(extraAttribute);
            }
            catch
            {
                return PartialView("Error404");
            }

        }

        [Authorize]
        // POST: Attributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ExtraAttribute extraAttribute = await db.ExtraAttributes.FindAsync(id);
                db.ExtraAttributes.Remove(extraAttribute);
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
