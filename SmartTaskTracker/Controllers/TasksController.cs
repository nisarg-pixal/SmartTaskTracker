using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.Models;
using SmartTaskTracker.Services;
using TaskStatus = SmartTaskTracker.Models.TaskStatus;

namespace SmartTaskTracker.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskService _service;

        public TasksController(TaskService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> ListPartial(string? filter)
        {
            var items = await _service.GetFilteredAsync(filter);
            return PartialView("_TasksTable", items);
        }

        public IActionResult Create()
        {
            return PartialView("_TaskForm", new TaskItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_TaskForm", task);
            }

            await _service.AddAsync(task);
            var items = await _service.GetAllAsync();
            return PartialView("_TasksTable", items);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _service.FindAsync(id);
            if (task == null) return NotFound();
            return PartialView("_TaskForm", task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem updated)
        {
            if (id != updated.Id) return BadRequest();
            if (!ModelState.IsValid) return PartialView("_TaskForm", updated);

            var task = await _service.FindAsync(id);
            if (task == null) return NotFound();

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.Status = updated.Status;
            task.StartTime = updated.StartTime;
            task.EndTime = updated.EndTime;
            task.Category = updated.Category;
            task.Deadline = updated.Deadline;

            await _service.UpdateAsync(task);
            var items = await _service.GetAllAsync();
            return PartialView("_TasksTable", items);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _service.FindAsync(id);
            if (task == null) return NotFound();
            return PartialView("_DeleteConfirm", task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _service.FindAsync(id);
            if (task == null) return NotFound();
            await _service.DeleteAsync(task);
            var items = await _service.GetAllAsync();
            return PartialView("_TasksTable", items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartTimer(int id)
        {
            var task = await _service.FindAsync(id);
            if (task == null) return NotFound();
            await _service.StartTimerAsync(task);
            var items = await _service.GetAllAsync();
            return PartialView("_TasksTable", items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StopTimer(int id)
        {
            var task = await _service.FindAsync(id);
            if (task == null) return NotFound();
            await _service.StopTimerAsync(task);
            var items = await _service.GetAllAsync();
            return PartialView("_TasksTable", items);
        }
    }
}


