using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApplication.WebApi.Data;
using ToDoApplication.WebApi.Models;
using ToDoApplication.WebApi.Services.Interfaces;

namespace ToDoApplication.WebApi.Services;

public class UserService : IUserService
{
    private readonly DatabaseContext _context;

    public UserService(DatabaseContext context)
    {
        _context = context;
    }
    
    public User CreateUser(User user)
    {
        if (_context.Users.Contains(user))
        {
            throw new Exception("User already created");
        }
        
        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public User GetUser(Guid userId)
    {
        return _context.Users.FirstOrDefault(x => x.Id == userId);
    }

    public bool AuthenticateUser(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user != null && user.Password == password)
        {
            return true;
        }

        return false;
    }

    public User RegisterUser(string name, string email, string password)
    {
        if (!IsEmailUnique(email))
        {
            throw new Exception("User already registered");
        }
        
        var user = new User
        {
            Email = email,
            Id = Guid.NewGuid(),
            Name = name,
            Password = password
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public byte[] GetUserProfilePhoto(Guid userId)
    {
        var profilePictureStoragePath = GetStoragePath();
        
        User user = _context.Users.FirstOrDefault(x => x.Id == userId);

        if (user != null)
        {
            string imagePath = Path.Combine(profilePictureStoragePath, userId.ToString());

            if (File.Exists(imagePath))
            {
                return File.ReadAllBytes(imagePath);
            }

            return null;
        }

        return null;
    }

    public bool SetUserProfilePhoto(Guid userId, IFormFile image)
    {
        var profilePictureStoragePath = GetStoragePath();

        User user = _context.Users.FirstOrDefault(x => x.Id == userId);

        if (user != null)
        {
            var imagePath = Path.Combine(profilePictureStoragePath, userId.ToString());

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                image.CopyToAsync(stream);
            }

            return true;
        }

        return false;
    }

    public Task<User> GetToDoListsForUsers(Guid userId)
    {
        // return _context.Users
        //     .Include(u => u.ToDoLists)
        //     .FirstOrDefaultAsync(u => u.Id == userId);
        return null;
    }
    
    public Task<List<ToDo>> GetToDoItemsForUser(Guid userId)
    {
        // return _context.ToDos
        //     .Where(todo => todo.ToDoList.UserId == userId)
        //     .ToListAsync();
        return null;
    }
    
    public bool IsEmailUnique(string email)
    {
        return !_context.Users.Any(u => u.Email == email);
    }

    public string GetStoragePath()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

        string profilePictureStoragePath = configuration.GetConnectionString("ProfilePictureStoragePath");
        return profilePictureStoragePath;
    }
}