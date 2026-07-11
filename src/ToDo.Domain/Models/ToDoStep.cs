namespace Domain.Models
{
    public class ToDoStep
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }


        public Guid TodoTaskId { get; set; }
        public ToDoTask TodoTask { get; set; } = null!;
    }
}
