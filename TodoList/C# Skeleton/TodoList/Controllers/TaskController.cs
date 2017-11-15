using System;
using System.Linq;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class TaskController : Controller
    {
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            using (var db = new TodoListDbContext())
            {
                var task = db.Tasks.ToList();

                return View(task);
            }
        }

        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Task task)
        {
            var taskParts = task;

            if (taskParts == null)
            {
                return View("Create");
            }

            if (taskParts.Title == null || taskParts.Comments == null)
            {
                return View("Create");
            }

            if (ModelState.IsValid)
            {
                using (var db = new TodoListDbContext())
                {
                    db.Tasks.Add(taskParts);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(task);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            using (var db = new TodoListDbContext())
            {
                var task = db.Tasks
                    .Where(a => a.Id == id)
                    .FirstOrDefault();

                if(task == null)
                {
                    return HttpNotFound();
                }

                return View(task);
            }
        }

        [HttpPost]
        [Route("delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var db = new TodoListDbContext())
            {
                var task = db.Tasks
                    .Where(a => a.Id == id)
                    .FirstOrDefault();

                if (task == null)
                {
                    return HttpNotFound();
                }

                db.Tasks.Remove(task);
                db.SaveChanges();

                return RedirectToAction("Index");
            }           
        }
    }
}