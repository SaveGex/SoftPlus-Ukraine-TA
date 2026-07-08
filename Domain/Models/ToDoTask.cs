using Domain.Enums;

namespace Domain.Models
{
    public class ToDoTask
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Note { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsImportant { get; set; }

        public bool IsMyDay { get; set; }

        public DateTime? ReminderAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public RecurrenceType Recurrence { get; set; } = RecurrenceType.None;

        public Guid? CategoryId { get; set; }
        public ToDoCategory? Category { get; set; } = null!;

        public ICollection<ToDoStep> Steps { get; set; } = new List<ToDoStep>();
    }
}
