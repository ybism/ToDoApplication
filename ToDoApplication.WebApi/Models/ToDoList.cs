namespace ToDoApplication.WebApi.Models;

public class ToDoList
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public List<ToDo>? ToDos { get; set; } // Each to-do list contains multiple to-dos

    // Foreign Key to User
    public Guid UserId { get; set; }
    public User? User { get; set; }
}