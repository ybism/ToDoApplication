using ToDoApplication.Models;
using Task = ToDoApplication.Models.Task;

namespace ToDoApplication.Interfaces;

public interface IUserService
{
    User CreateUser(User user);
    User LoginUser(string email, string password);
    User UpdateUser(int userId, User updatedUser);
    bool SetUserProfilePhoto(int id, IFormFile profilePhoto);
    void StreamUserProfilePhoto(int userId, Stream outputStream);
    IEnumerable<TaskList> GetUserTaskLists(int userId);
    IEnumerable<Task> GetUserTaskListsWithTasks(int userId);
    TaskList CreateTaskList(int id, TaskList taskList);
    Task CreateTask(int userId, int taskListId, Task task);
    bool RemoveTask(int userId, int taskId);
    bool EditTask(int userId, int taskId, Task updatedTask);
    int[] GetTaskStatusCounts(int userId);
}
