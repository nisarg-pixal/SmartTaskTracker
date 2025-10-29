using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.Data;
using SmartTaskTracker.Models;
using TaskStatus = SmartTaskTracker.Models.TaskStatus;

namespace SmartTaskTracker.Services
{
    public class TaskService
    {
        private readonly AppDbContext _db;
        public TaskService(AppDbContext db) { _db = db; }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _db.Tasks.AsNoTracking().OrderByDescending(t => t.Id).ToListAsync();
        }

        public async Task<List<TaskItem>> GetFilteredAsync(string? filter)
        {
            var q = _db.Tasks.AsNoTracking().AsQueryable();
            switch ((filter ?? string.Empty).ToLower())
            {
                case "completed":
                    q = q.Where(t => t.Status == TaskStatus.Completed);
                    break;
                case "pending":
                    q = q.Where(t => t.Status == TaskStatus.Pending);
                    break;
                case "today":
                    var today = DateTime.Today;
                    q = q.Where(t => t.StartTime.HasValue && t.StartTime.Value.Date == today);
                    break;
                default:
                    break;
            }
            return await q.OrderByDescending(t => t.Id).ToListAsync();
        }

        public async Task AddAsync(TaskItem item)
        {
            _db.Tasks.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task<TaskItem?> FindAsync(int id) => await _db.Tasks.FindAsync(id);

        public async Task UpdateAsync(TaskItem item)
        {
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem item)
        {
            _db.Tasks.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task StartTimerAsync(TaskItem task)
        {
            task.Status = TaskStatus.InProgress;
            task.StartTime = DateTime.Now;
            task.EndTime = null;
            await _db.SaveChangesAsync();
        }

        public async Task StopTimerAsync(TaskItem task)
        {
            task.EndTime = DateTime.Now;
            task.Status = TaskStatus.Completed;
            await _db.SaveChangesAsync();
        }
    }
}


