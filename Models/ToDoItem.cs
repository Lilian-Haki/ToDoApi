namespace ToDoApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        public string Priority { get; set; } = "Medium"; // Low, Medium, High
        public DateTime? DueDate { get; set; }
        public string Category { get; set; } = "General";

        public int UserId { get; set; }
        public User? User { get; set; }
    }



}
