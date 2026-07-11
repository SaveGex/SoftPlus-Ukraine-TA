namespace Domain.Models
{
    public class ToDoCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Icon { get; set; }

        public ICollection<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
        public Guid UserId { get; set; }
    }
}
