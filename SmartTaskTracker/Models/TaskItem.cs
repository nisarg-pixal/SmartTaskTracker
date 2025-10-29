using System.ComponentModel.DataAnnotations;

namespace SmartTaskTracker.Models
{
    public enum TaskStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }

    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Display(Name = "Status")]
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        [Display(Name = "Start Time")]
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }

        [Display(Name = "End Time")]
        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }

        [StringLength(64)]
        public string? Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Deadline { get; set; }

        public bool IsTimerRunning => StartTime.HasValue && !EndTime.HasValue && Status == TaskStatus.InProgress;

        public bool IsCompleted => Status == TaskStatus.Completed;
    }
}


