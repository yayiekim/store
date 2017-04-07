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
    public class CustomerShippingAddressesController : Controller
    {
        private Entities db = new Entities();

        // GET: CustomerShippingAddresses
        public async Task<ActionResult> Index()
        {
            var customerShippingAddresses = db.CustomerShippingAddresses.Include(c => c.AspNetUser);
            return View(await customerShippingAddresses.ToListAsync());
        }

        // GET: CustomerShippingAddresses/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerShippingAddress customerShippingAddress = await db.CustomerShippingAddresses.FindAsync(id);
            if (customerShippingAddress == null)
            {
                return HttpNotFound();
            }
            return View(customerShippingAddress);
        }

        // GET: CustomerShippingAddresses/Create
        public ActionResult Create()
        {
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: CustomerShippingAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AspNetUserId,IsDefault,Line1,Line2,City,State")] CustomerShippingAddress customerShippingAddress)
        {
            if (ModelState.IsValid)
            {
                db.CustomerShippingAddresses.Add(customerShippingAddress);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", customerShippingAddress.AspNetUserId);
            return View(customerShippingAddress);
        }

        // GET: CustomerShippingAddresses/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerShippingAddress customerShippingAddress = await db.CustomerShippingAddresses.FindAsync(id);
            if (customerShippingAddress == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", customerShippingAddress.AspNetUserId);
            return View(customerShippingAddress);
        }

        // POST: CustomerShippingAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AspNetUserId,IsDefault,Line1,Line2,City,State")] CustomerShippingAddress customerShippingAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerShippingAddress).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", customerShippingAddress.AspNetUserId);
            return View(customerShippingAddress);
        }

        // GET: CustomerShippingAddresses/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerShippingAddress customerShippingAddress = await db.CustomerShippingAddresses.FindAsync(id);
            if (customerShippingAddress == null)
            {
                return HttpNotFound();
            }
            return View(customerShippingAddress);
        }

        // POST: CustomerShippingAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            CustomerShippingAddress customerShippingAddress = await db.CustomerShippingAddresses.FindAsync(id);
            db.CustomerShippingAddresses.Remove(customerShippingAddress);
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
