namespace ToDoApplication.WebApi.Models;

public class ToDo
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? InProgress { get; set; }
    public bool? IsComplete { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;

    public ToDoList? ToDoList { get; set; } 
    public Guid? ToDoListId{ get; set; }
}