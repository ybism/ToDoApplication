namespace ToDoApplication.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool InProgress { get; set; }
    public bool IsComplete { get; set; }
    public int? UserId { get; set; }
    public int? TaskListId { get; set; } 
    public User? User { get; set; } 
    public TaskList? TaskList { get; set; } 
}