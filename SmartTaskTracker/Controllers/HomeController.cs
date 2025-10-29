using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.Data;
using SmartTaskTracker.Models;
using SmartTaskTracker.Services;
using TaskStatus = SmartTaskTracker.Models.TaskStatus;

namespace SmartTaskTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly MotivationService _motivationService;

        public HomeController(AppDbContext db, MotivationService motivationService)
        {
            _db = db;
            _motivationService = motivationService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _db.Tasks.AsNoTracking().ToListAsync();

            var total = tasks.Count;
            var completed = tasks.Count(t => t.Status == TaskStatus.Completed);
            var active = tasks.Count(t => t.Status != TaskStatus.Completed);
            var productivity = total == 0 ? 0 : (int)Math.Round((double)completed / total * 100);

            ViewBag.TotalTasks = total;
            ViewBag.CompletedTasks = completed;
            ViewBag.ActiveTasks = active;
            ViewBag.Productivity = productivity;
            ViewBag.Motivation = _motivationService.GetRandomMessage();

            // For the weekly chart, group by day for last 7 days
            var end = DateTime.Today.AddDays(1);
            var start = end.AddDays(-7);
            var recentCompleted = tasks
                .Where(t => t.Status == TaskStatus.Completed && t.EndTime.HasValue && t.EndTime.Value >= start && t.EndTime.Value < end)
                .ToList();

            var series = Enumerable.Range(0, 7)
                .Select(i => start.AddDays(i))
                .Select(d => new
                {
                    day = d.ToString("ddd"),
                    count = recentCompleted.Count(t => t.EndTime!.Value.Date == d.Date)
                })
                .ToList();

            ViewBag.WeeklyLabels = series.Select(s => s.day).ToArray();
            ViewBag.WeeklyCounts = series.Select(s => s.count).ToArray();

            return View();
        }

        [HttpGet]
        public IActionResult Quote()
        {
            return Json(new { message = _motivationService.GetRandomMessage() });
        }
    }
}


