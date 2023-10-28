namespace ToDoApplication.WebApi.Models;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; } // Be sure to properly hash and salt passwords
    public string? ProfilePhotoUrl { get; set; } // Store the URL to the user's profile photo
    public List<ToDoList>? ToDoLists { get; set; } // A user can have multiple to-do lists

}