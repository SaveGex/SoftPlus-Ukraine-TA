namespace Domain.Models
{
    public class ToDoCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? IconId { get; set; }
        public Icon? Icon { get; set; }

        public ICollection<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
        public Guid AuthorId { get; set; }
    }
}
