namespace ToDoApplication.Models;

public class TaskList
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<Task>? Tasks { get; set; } // Navigation property for tasks in the list
    public int? UserId { get; set; } // Foreign key for the user who owns the list
    public User? User { get; set; } // Navigation property for the user who owns the list
}