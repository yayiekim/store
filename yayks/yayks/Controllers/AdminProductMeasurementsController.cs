using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using yayks;

namespace yayks.Controllers
{
    public class AdminProductMeasurementsController : Controller
    {
        private Entities db = new Entities();

        // GET: AdminProductMeasurements
        public async Task<ActionResult> Index()
        {
            var productMeasurements = db.ProductMeasurements.Include(p => p.ProductCategory);
            return View(await productMeasurements.ToListAsync());
        }

        // GET: AdminProductMeasurements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMeasurement productMeasurement = await db.ProductMeasurements.FindAsync(id);
            if (productMeasurement == null)
            {
                return HttpNotFound();
            }
            return View(productMeasurement);
        }

        // GET: AdminProductMeasurements/Create
        public ActionResult Create()
        {
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "CategoryName");
            return View();
        }

        // POST: AdminProductMeasurements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MeasurementName,ProductCategoryId,MeasurementValue")] ProductMeasurement productMeasurement)
        {
            if (ModelState.IsValid)
            {
                db.ProductMeasurements.Add(productMeasurement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "CategoryName", productMeasurement.ProductCategoryId);
            return View(productMeasurement);
        }

        // GET: AdminProductMeasurements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMeasurement productMeasurement = await db.ProductMeasurements.FindAsync(id);
            if (productMeasurement == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "CategoryName", productMeasurement.ProductCategoryId);
            return View(productMeasurement);
        }

        // POST: AdminProductMeasurements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MeasurementName,ProductCategoryId,MeasurementValue")] ProductMeasurement productMeasurement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productMeasurement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "CategoryName", productMeasurement.ProductCategoryId);
            return View(productMeasurement);
        }

        // GET: AdminProductMeasurements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductMeasurement productMeasurement = await db.ProductMeasurements.FindAsync(id);
            if (productMeasurement == null)
            {
                return HttpNotFound();
            }
            return View(productMeasurement);
        }

        // POST: AdminProductMeasurements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductMeasurement productMeasurement = await db.ProductMeasurements.FindAsync(id);
            db.ProductMeasurements.Remove(productMeasurement);
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
