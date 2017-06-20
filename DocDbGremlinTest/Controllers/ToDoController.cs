using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocDbGremlinTest.Data;
using DocDbGremlinTest.Models;

namespace DocDbGremlinTest.Controllers
{
    public class ToDoController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var items = await DocumentDbRepository<ToDo>.GetItemsAsync(d => !d.Completed);
            return View(items);
        }

#pragma warning disable 1998
        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }
#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Name,Description,Completed")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                await DocumentDbRepository<ToDo>.CreateItemAsync(toDo);
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Name,Description,Completed")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                await DocumentDbRepository<ToDo>.UpdateItemAsync(toDo.Id, toDo);
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ToDo toDo = await DocumentDbRepository<ToDo>.GetItemAsync(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }

            return View(toDo);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ToDo toDo = await DocumentDbRepository<ToDo>.GetItemAsync(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }

            return View(toDo);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id")] string id)
        {
            await DocumentDbRepository<ToDo>.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            ToDo toDo = await DocumentDbRepository<ToDo>.GetItemAsync(id);
            return View(toDo);
        }
    }
}