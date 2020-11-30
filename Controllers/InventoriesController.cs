using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PartsInventoryV6;
using System.Threading.Tasks;
using PagedList;

//File was made with the help of: https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
namespace PartsInventoryV6.Controllers
{
    public class InventoriesController : Controller
    {
        private EquipmentContext db = new EquipmentContext();

        // GET: Inventories
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // using System;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DescSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NewNumSort = sortOrder == "newNum" ? "newNum_desc" : "newNum";
            ViewBag.OldNumSort = sortOrder == "oldNum" ? "oldNum_desc" : "oldNum";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var parts = from part in db.Inventories
                        select part;

            if (!String.IsNullOrEmpty(searchString))
            {
                parts = parts.Where(part => part.DESCRIPTION.Contains(searchString)
                                       || part.OLD_NUMBER.Contains(searchString)
                                       || part.NEW_NUMBER.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc": //Sort by name descending
                    parts = parts.OrderByDescending(part => part.DESCRIPTION);
                    break;
                case "newNum": //Sort by new number aescending
                    parts = parts.OrderBy(part => part.NEW_NUMBER);
                    break;
                case "newNum_desc": //Sort by new number descending
                    parts = parts.OrderByDescending(part => part.NEW_NUMBER);
                    break;
                case "oldNum": //Sort by old number
                    parts = parts.OrderBy(part => part.OLD_NUMBER);
                    break;
                case "oldNum_desc": //Sort by old number descending
                    parts = parts.OrderByDescending(part => part.OLD_NUMBER);
                    break;
                default: //Default to ordering by part ID
                    parts = parts.OrderBy(part => part.ID);
                    break;

            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(parts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Inventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DESCRIPTION,OLD_NUMBER,NEW_NUMBER,UNIT_OF_ISSUE,SYS_CODE,IMAGE_PATH")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DESCRIPTION,OLD_NUMBER,NEW_NUMBER,UNIT_OF_ISSUE,SYS_CODE,IMAGE_PATH")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
            db.SaveChanges();
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
