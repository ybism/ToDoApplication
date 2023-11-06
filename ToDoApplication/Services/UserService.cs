using ToDoApplication.Data;
using ToDoApplication.Interfaces;
using ToDoApplication.Models;
using Task = ToDoApplication.Models.Task;

namespace ToDoApplication.Services;

public class UserService : IUserService
{
    private readonly DatabaseContext _dbContext;

    public UserService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User CreateUser(User user)
    {
        var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return user;
    }

    public User LoginUser(string email, string password)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
    }

    public User UpdateUser(int userId, User updatedUser)
    {
        var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

        if (existingUser != null)
        {
            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;
            existingUser.PasswordHash = updatedUser.PasswordHash;
            existingUser.ProfileImage = updatedUser.ProfileImage;

            _dbContext.SaveChanges();
        }

        return existingUser;
    }

    public bool SetUserProfilePhoto(int id, IFormFile profilePhoto)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        var profilePictureStoragePath = GetStoragePath();
        
        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        var imagePath = Path.Combine(profilePictureStoragePath, id.ToString());

        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            profilePhoto.CopyToAsync(stream);
        }

        user.ProfileImage = imagePath;
        _dbContext.Update(user);
        _dbContext.SaveChanges();

        return true;
    }

    public void StreamUserProfilePhoto(int userId, Stream outputStream)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        var profilePictureStoragePath = GetStoragePath();
        string imagePath = Path.Combine(profilePictureStoragePath, userId.ToString());

        if (File.Exists(imagePath))
        {
            using (FileStream fileStream = File.OpenRead(imagePath))
            {
                fileStream.CopyTo(outputStream);
            }
        }
        else
        {
            throw new Exception("The file does not exist");
        }
    }
    
    public IEnumerable<TaskList> GetUserTaskLists(int userId)
    {
        return _dbContext.TaskLists.Where(tl => tl.UserId == userId).ToList();
    }

    public IEnumerable<Task> GetUserTaskListsWithTasks(int userId)
    {
        return _dbContext.Tasks.Where(t => t.UserId == userId).ToList();
    }
    
    public TaskList CreateTaskList(int userId, TaskList taskList)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        var existingTaskList = _dbContext.TaskLists
            .FirstOrDefault(tl => tl.UserId == userId && tl.Title == taskList.Title);

        if (existingTaskList != null)
        {
            throw new Exception("List with the same name already exists");
        }

        taskList.UserId = userId;
        taskList.User = user;

        _dbContext.TaskLists.Add(taskList);
        _dbContext.SaveChanges();

        return taskList;
    }
    
    public Task CreateTask(int userId, int taskListId, Task task)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        var taskList = _dbContext.TaskLists.FirstOrDefault(tl => tl.Id == taskListId);

        if (user == null || taskList == null)
        {
            // User or task list not found
            throw new Exception("Either the user or task list was not found");
        }

        task.UserId = userId;
        task.TaskListId = taskListId;
        task.User = user;
        task.TaskList = taskList;
        task.CreatedAt = DateTime.Now;
        if (task.DueDate == DateTime.MinValue)
        {
            task.DueDate = DateTime.Today.AddDays(7); 
        }

        _dbContext.Tasks.Add(task);
        _dbContext.SaveChanges();

        return task;
    }

    public bool RemoveTask(int userId, int taskId)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User does not exist");
        }

        var taskToRemove = _dbContext.Tasks.FirstOrDefault(t => t.Id == taskId && t.UserId == userId);

        if (taskToRemove == null)
        {
            throw new Exception("Task does not exist");
        }

        _dbContext.Tasks.Remove(taskToRemove);
        _dbContext.SaveChanges();

        return true;
    }

    public bool EditTask(int userId, int taskId, Task updatedTask)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            // User not found
            return false;
        }

        var taskToEdit = _dbContext.Tasks.FirstOrDefault(t => t.Id == taskId && t.UserId == userId);

        if (taskToEdit == null)
        {
            // Task not found for the specified user
            return false;
        }

        // Update the properties of the existing task with the new values
        taskToEdit.Title = updatedTask.Title;
        taskToEdit.Description = updatedTask.Description;
        taskToEdit.CreatedAt = updatedTask.CreatedAt;
        taskToEdit.DueDate = updatedTask.DueDate;
        taskToEdit.InProgress = updatedTask.InProgress;
        taskToEdit.IsComplete = updatedTask.IsComplete;

        _dbContext.Update(taskToEdit);
        _dbContext.SaveChanges();

        return true;
    }
    
    
    public int[] GetTaskStatusCounts(int userId)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            return null;
        }
        
        var tasks = _dbContext.Tasks
            .Where(t => t.UserId == userId) // Filter by the user's ID.
            .ToList();

        var notInProgressCount = tasks.Count(t => !t.InProgress && !t.IsComplete);
        var inProgressCount = tasks.Count(t => t.InProgress && !t.IsComplete);
        var completedCount = tasks.Count(t => t.IsComplete);

        var result = new[] { notInProgressCount, inProgressCount, completedCount };

        return result;
    }
    
    private string GetStoragePath()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

        string profilePictureStoragePath = configuration.GetConnectionString("ProfilePictureStoragePath");
        return profilePictureStoragePath;
    }
}
