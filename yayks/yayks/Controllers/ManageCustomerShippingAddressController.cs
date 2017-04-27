using System;
using Microsoft.AspNet.Identity;
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
    public class ManageCustomerShippingAddressController : Controller
    {
        private Entities db = new Entities();

        // GET: ManageCustomerShippingAddress
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var customerShippingAddresses = db.CustomerShippingAddresses.Where(i => i.AspNetUserId == userId);           
            return View(await customerShippingAddresses.ToListAsync());
        }

        // GET: ManageCustomerShippingAddress/Details/5
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

        // GET: ManageCustomerShippingAddress/Create
        public ActionResult Create()
        {
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: ManageCustomerShippingAddress/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AspNetUserId,IsDefault,Line1,Line2,City,State")] CustomerShippingAddress customerShippingAddress)
        {
            if (ModelState.IsValid)
            {
                customerShippingAddress.Id = Guid.NewGuid().ToString();
                db.CustomerShippingAddresses.Add(customerShippingAddress);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", customerShippingAddress.AspNetUserId);
            return View(customerShippingAddress);
        }

        // GET: ManageCustomerShippingAddress/Edit/5
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

        // POST: ManageCustomerShippingAddress/Edit/5
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

        // GET: ManageCustomerShippingAddress/Delete/5
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

        // POST: ManageCustomerShippingAddress/Delete/5
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
