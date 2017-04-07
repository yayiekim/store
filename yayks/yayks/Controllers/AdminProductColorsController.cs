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
    public class AdminProductColorsController : Controller
    {
        private Entities db = new Entities();

        // GET: AdminProductColors
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductColors.ToListAsync());
        }

        // GET: AdminProductColors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = await db.ProductColors.FindAsync(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }

        // GET: AdminProductColors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminProductColors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductColorName,ProductColorCode")] ProductColor productColor)
        {
            if (ModelState.IsValid)
            {
                db.ProductColors.Add(productColor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productColor);
        }

        // GET: AdminProductColors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = await db.ProductColors.FindAsync(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }

        // POST: AdminProductColors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductColorName,ProductColorCode")] ProductColor productColor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productColor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productColor);
        }

        // GET: AdminProductColors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = await db.ProductColors.FindAsync(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }

        // POST: AdminProductColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductColor productColor = await db.ProductColors.FindAsync(id);
            db.ProductColors.Remove(productColor);
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
