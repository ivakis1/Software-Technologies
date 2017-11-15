
namespace BlogCSharp.Controllers
{
    using Microsoft.AspNet.Identity;
    using Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class ArticleController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var db = new BlogDbContext())
            {
                var article = db.Articles.Where(a => a.Id == id).FirstOrDefault();

                if(article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(int id)
        {
            using (var db = new BlogDbContext())
            {
                var artcile = db.Articles.Where(a => a.Id == id).FirstOrDefault();

                if(artcile == null)
                {
                    return HttpNotFound();
                }

                db.Articles.Remove(artcile);
                db.SaveChanges();
            }

            return RedirectToAction("List", "Article");

        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new BlogDbContext())
            {
                var article = db.Articles.Find(id);              

                var articleViewModel = new ArticleViewModel
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    AuthorId = article.AuthorId
                };

                return View(articleViewModel);
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogDbContext())
                {
                    var article = db.Articles.Find(model.Id);

                    article.Title = model.Title;
                    article.Content = model.Content;

                    db.SaveChanges();

                }
                return RedirectToAction("Details", new { id = model.Id });
            }

            return View(model);
        }
        public ActionResult List()
        {
            using(var db = new BlogDbContext())
            {
                var artciles = db.Articles.Include(a => a.Author).ToList();

                return View(artciles);
            }
        }

        [Authorize]
        [HttpGet]
       public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogDbContext())
                {
                    var authorId = User.Identity.GetUserId();

                    article.AuthorId = authorId;

                    db.Articles.Add(article);
                    db.SaveChanges();
                }

                return RedirectToAction("List");
            }

            return View(article);
        }
        public ActionResult Details(int? id)
        {
            using (var db = new BlogDbContext())
            {
                var article = db.Articles
                    .Include(a => a.Author)
                    .Where(a => a.Id == id)
                    .FirstOrDefault();

                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }
    }
}