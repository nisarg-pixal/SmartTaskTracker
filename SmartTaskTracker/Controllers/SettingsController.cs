using Microsoft.AspNetCore.Mvc;
using SmartTaskTracker.Data;
using SmartTaskTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartTaskTracker.Controllers
{
    public class SettingsController : Controller
    {
        private readonly AppDbContext _db;
        public SettingsController(AppDbContext db){ _db = db; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset()
        {
            // Clear and reseed
            _db.Tasks.RemoveRange(_db.Tasks);
            await _db.SaveChangesAsync();
            DbSeeder.Seed(_db);
            TempData["ResetMessage"] = "All tasks have been reset.";
            return RedirectToAction("Index");
        }
    }
}


