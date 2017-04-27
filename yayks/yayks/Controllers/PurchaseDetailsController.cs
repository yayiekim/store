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
    public class PurchaseDetailsController : Controller
    {
        private Entities db = new Entities();

        // GET: PurchaseDetails
        public async Task<ActionResult> Index(string id)
        {
            var purchaseDetails = db.PurchaseDetails.Where(i=>i.PurchaseId == id);
            ViewBag.PurchaseId = id;

            return View(await purchaseDetails.ToListAsync());
        }

        // GET: PurchaseDetails/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetail purchaseDetail = await db.PurchaseDetails.FindAsync(id);
            if (purchaseDetail == null)
            {
                return HttpNotFound();
            }
            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Create
        public ActionResult Create(string id)
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");           
            ViewBag.PurchaseId = id;

            return View();
        }

        // POST: PurchaseDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PurchaseId,ProductId,Amount,Quantity,Remarks,PurchaseDetailStatus")] PurchaseDetail purchaseDetail)
        {
            purchaseDetail.Id = Guid.NewGuid().ToString();

            if (ModelState.IsValid)
            {
                db.PurchaseDetails.Add(purchaseDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "PurchaseDetails", new { id = purchaseDetail.PurchaseId });
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", purchaseDetail.ProductId);
            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Edit/5
        public async Task<ActionResult> Edit(string id)
        {           
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetail purchaseDetail = await db.PurchaseDetails.FindAsync(id);
            if (purchaseDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", purchaseDetail.ProductId);
 
            return View(purchaseDetail);
        }

        // POST: PurchaseDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Product,PurchaseId,ProductId,Amount,Quantity,Remarks,PurchaseDetailStatus")] PurchaseDetail purchaseDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index","PurchaseDetails", new { id = purchaseDetail.PurchaseId });
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", purchaseDetail.ProductId);
            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetail purchaseDetail = await db.PurchaseDetails.FindAsync(id);
            if (purchaseDetail == null)
            {
                return HttpNotFound();
            }
            return View(purchaseDetail);
        }

        // POST: PurchaseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            PurchaseDetail purchaseDetail = await db.PurchaseDetails.FindAsync(id);
            db.PurchaseDetails.Remove(purchaseDetail);
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
