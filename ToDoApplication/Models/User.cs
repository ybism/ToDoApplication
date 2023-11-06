namespace ToDoApplication.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? ProfileImage { get; set; }
    public ICollection<Task>? Tasks { get; set; } 
    public ICollection<TaskList>? TaskLists { get; set; }
}