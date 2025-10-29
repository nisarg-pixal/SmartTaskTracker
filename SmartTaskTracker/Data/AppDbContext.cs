using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.Models;
using TaskStatus = SmartTaskTracker.Models.TaskStatus;

namespace SmartTaskTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
    }

    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Tasks.Any()) return;

            var now = DateTime.Now;
            var seed = new List<TaskItem>
            {
                new TaskItem { Title = "Plan the week", Description = "Outline key tasks", Status = TaskStatus.Completed, StartTime = now.AddDays(-2).AddHours(-1), EndTime = now.AddDays(-2) },
                new TaskItem { Title = "Implement dashboard", Description = "Build cards, progress bar", Status = TaskStatus.InProgress, StartTime = now.AddHours(-2) },
                new TaskItem { Title = "Write unit tests", Description = "Cover core services", Status = TaskStatus.Pending },
                new TaskItem { Title = "Refactor controllers", Description = "Cleanup actions", Status = TaskStatus.Completed, StartTime = now.AddDays(-1).AddHours(-3), EndTime = now.AddDays(-1).AddHours(-1) },
            };

            db.Tasks.AddRange(seed);
            db.SaveChanges();
        }
    }
}


