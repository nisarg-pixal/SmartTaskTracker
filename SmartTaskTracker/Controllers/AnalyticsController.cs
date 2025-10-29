using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.Data;
using SmartTaskTracker.Models;
using TaskStatus = SmartTaskTracker.Models.TaskStatus;

namespace SmartTaskTracker.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly AppDbContext _db;

        public AnalyticsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _db.Tasks.AsNoTracking().ToListAsync();
            var total = tasks.Count;
            var completed = tasks.Count(t => t.Status == TaskStatus.Completed);
            var productivity = total == 0 ? 0 : (int)Math.Round((double)completed / total * 100);

            ViewBag.Productivity = productivity;

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

            // Doughnut data by status
            var byStatus = new[] {
                tasks.Count(t => t.Status == TaskStatus.Pending),
                tasks.Count(t => t.Status == TaskStatus.InProgress),
                tasks.Count(t => t.Status == TaskStatus.Completed)
            };
            ViewBag.StatusCounts = byStatus;

            // Insight: most productive day by completions
            var completionsByDay = recentCompleted
                .GroupBy(t => t.EndTime!.Value.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();
            ViewBag.InsightTopDay = completionsByDay?.Day.ToString() ?? "N/A";
            ViewBag.InsightTip = completed < total ? "Try finishing more before noon!" : "Great consistency!";

            return View();
        }

        public async Task<IActionResult> Report()
        {
            var tasks = await _db.Tasks.AsNoTracking().ToListAsync();
            var total = tasks.Count;
            var completed = tasks.Count(t => t.Status == TaskStatus.Completed);
            var productivity = total == 0 ? 0 : (int)Math.Round((double)completed / total * 100);

            ViewBag.Productivity = productivity;

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
    }
}


