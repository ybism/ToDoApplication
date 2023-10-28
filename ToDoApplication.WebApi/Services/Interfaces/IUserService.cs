using ToDoApplication.WebApi.Models;

namespace ToDoApplication.WebApi.Services.Interfaces;

public interface IUserService
{
    User CreateUser(User user);
    User GetUser(Guid userId);
    bool AuthenticateUser(string email, string password);
    User RegisterUser(string name, string email, string password);
    byte[] GetUserProfilePhoto(Guid userId);
    bool SetUserProfilePhoto(Guid userId, IFormFile image);
    Task<User> GetToDoListsForUsers(Guid userId);
    public Task<List<ToDo>> GetToDoItemsForUser(Guid userId);
}
