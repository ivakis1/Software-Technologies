using System;
using System.Linq;
using System.Web.Mvc;
using TeisterMask.Models;

namespace TeisterMask.Controllers
{
    [ValidateInput(false)]
    public class TaskController : Controller
    {
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            using (var db = new TeisterMaskDbContext())
            {
                var task = db.Tasks.ToList();

                return View(task);
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
        public ActionResult Create(Task task)
        {
            var tastToCreate = task;

            if (ModelState.IsValid)
            {

                using (var db = new TeisterMaskDbContext())
                {
                    db.Tasks.Add(tastToCreate);
                    db.SaveChanges();

                    return Redirect("Index");
                }
            }

            return View();
        }

        [HttpGet]
        [Route("edit/{id}")]
        public ActionResult Edit(int id)
        {
            using (var db = new TeisterMaskDbContext())
            {
                var task = db.Tasks.Find(id);

                if (task == null)
                {
                    return Redirect("Index");
                }

                return View(task);
            }
        }

        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfirm(int id, Task taskModel)
        {
            using (var db = new TeisterMaskDbContext())
            {
                var currentTask = db.Tasks.Find(id);

                if (currentTask == null)
                {
                    return RedirectToAction("Index");
                }

                currentTask.Title = taskModel.Title;
                currentTask.Status = taskModel.Status;

                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }
    }
}