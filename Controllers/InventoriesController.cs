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
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;


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
        public ActionResult Create([Bind(Include = "ID,DESCRIPTION,OLD_NUMBER,NEW_NUMBER,UNIT_OF_ISSUE,SYS_CODE,IMAGE_PATH")] Inventory inventory, HttpPostedFileBase postedFile)
        {
            //Check to see if the NEW_NUMBER already exists in the DB by looking for a match.
            var matchingPart = from part in db.Inventories
                        where part.NEW_NUMBER == inventory.NEW_NUMBER
                        select part.NEW_NUMBER;

            var match = "";
            System.Diagnostics.Debug.WriteLine("Printing match:...");
            foreach (var row in matchingPart.ToList())
            {
                Console.WriteLine("Match:" + row.ToString());
                System.Diagnostics.Debug.WriteLine("Match:" + row.ToString());
                match = row.ToString();
            }

            //Pretty sure that match is not an IEnumerable variable but idk what to do with it

            string path = Server.MapPath("~/Images/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                System.Diagnostics.Debug.WriteLine("Created the folder.");
            }
            System.Diagnostics.Debug.WriteLine("First check:" + match);
            if (postedFile != null && match == "")
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                string pattern = @".+?(?=\.)";
                string final = Regex.Replace(fileName, pattern, inventory.NEW_NUMBER);
                inventory.IMAGE_PATH = final;
                postedFile.SaveAs(path + final);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }
 
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Second check:" + match);
                if (match == "")
                {
                    db.Inventories.Add(inventory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Response.Write("<script>alert('That New Number already exists. There cannot be duplicate New Numbers.')</script>");
                }
            }

            return View(inventory);
        }

        public ViewResult AdminIndex(string sortOrder, string currentFilter, string searchString, int? page)
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
        public ActionResult Edit([Bind(Include = "ID,DESCRIPTION,OLD_NUMBER,NEW_NUMBER,UNIT_OF_ISSUE,SYS_CODE,IMAGE_PATH")] Inventory inventory, HttpPostedFileBase postedFile)
        {
            string path = Server.MapPath("~/Images/");
            if (postedFile != null)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                string pattern = @".+(?=\.)";
                string final = Regex.Replace(fileName, pattern, inventory.NEW_NUMBER);
                inventory.IMAGE_PATH = final;
                string saveFile = path + final;
                //Look for the the already associated image and check jpg png and gif and if it exists delete it.
                if (System.IO.File.Exists(path + inventory.NEW_NUMBER + ".jpg"))
                {
                    System.IO.File.Delete(path + inventory.NEW_NUMBER + ".jpg");
                }
                if (System.IO.File.Exists(path + inventory.NEW_NUMBER + ".png"))
                {
                    System.IO.File.Delete(path + inventory.NEW_NUMBER + ".png");
                }
                if (System.IO.File.Exists(path + inventory.NEW_NUMBER + ".gif"))
                {
                    System.IO.File.Delete(path + inventory.NEW_NUMBER + ".gif");
                }
                postedFile.SaveAs(saveFile);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }


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
            
            string jpgName = inventory.NEW_NUMBER + ".jpg";
            var filePath = Server.MapPath("~/Images/" + jpgName);
            //If file is a .jpg
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else //if the file is a .png
            {
                string pngName = inventory.NEW_NUMBER + ".png";
                var newPath = Server.MapPath("~/Images/" + pngName);
                System.IO.File.Delete(newPath);
            }

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
