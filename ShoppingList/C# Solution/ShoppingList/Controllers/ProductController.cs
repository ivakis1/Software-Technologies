using System.Linq;
using System.Web.Mvc;
using ShoppingList.Models;

namespace ShoppingList.Controllers
{
    [ValidateInput(false)]
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            using (var db = new ShoppingListDbContext())
            {
                var shoppingList = db.Products.ToList();

                return View(shoppingList);
            }
        }

        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ShoppingListDbContext())
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        [Route("edit/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            using (var db = new ShoppingListDbContext())
            {
                var product = db.Products.Find(id);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                return View(product);
            }
        }

        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfirm(int? id, Product productModel)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            using (var db = new ShoppingListDbContext())
            {
                var product = db.Products.Find(id);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                product.Name = productModel.Name;
                product.Priority = productModel.Priority;
                product.Quantity = productModel.Quantity;
                product.Status = productModel.Status;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
