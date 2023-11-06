using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.Interfaces;
using ToDoApplication.Models;
using Task = ToDoApplication.Models.Task;

namespace ToDoApplication.Controllers;

//TODO Implement Validation
//TODO Documentation
//TODO Implement custom error classes

[ApiController]
[Route("[controller]")]
[EnableCors("AllowReactApp")]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("createUser")]
    public IActionResult CreateUser([FromBody] User user)
    {
        try
        {
            var createdUser = _userService.CreateUser(user);
            return Ok(createdUser.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("loginUser/{email}/{password}")]
    public IActionResult LoginUser(string email, string password)
    {
        var authenticatedUser = _userService.LoginUser(email, password);

        if (authenticatedUser == null)
        {
            return Unauthorized("User not found or invalid credentials");
        }

        return Ok(new { userId = authenticatedUser.Id });
    }

    [HttpPost("setphoto")]
    public IActionResult SetUserProfilePhoto(int userId, IFormFile image)
    {
        var photoUploadSuccess = _userService.SetUserProfilePhoto(userId, image);

        if (!photoUploadSuccess)
        {
            return BadRequest();
        }

        return Ok(userId);
    }

[HttpGet("retrieveProfilePhoto/{userId}")]
public IActionResult StreamUserProfilePhoto(int userId)
{
    Stream photoStream = new MemoryStream();
    _userService.StreamUserProfilePhoto(userId, photoStream);

    if (photoStream.Length > 0)
    {
        photoStream.Position = 0; // Reset the stream position to the beginning
        return File(photoStream, "image/jpeg"); // Set the appropriate content type
    }

    return NotFound(); // You can return NotFound when the image is not found
}

    [HttpGet("GetAllUserTaskLists/{userId}")]
    public IActionResult GetUserTaskLists(int userId)
    {
        var userTaskLists = _userService.GetUserTaskLists(userId);
        
        if (userTaskLists.Any())
        {
            return Ok(userTaskLists);
        }
        
        return BadRequest("User has no task lists");
    }
    
    [HttpGet("GetAllUserTasks/{userId}")]
    public IActionResult GetUserTasks(int userId)
    {
        var userTaskLists = _userService.GetUserTaskListsWithTasks(userId);
        
        if (userTaskLists.Any())
        {
            return Ok(userTaskLists);
        }
        
        return BadRequest("User has no task lists or tasks");
    }

    [HttpPost("CreateOneTaskList/{userId}")]
    public IActionResult CreateTaskListForUser(int userId, TaskList taskList)
    {
        var taskListCreated = _userService.CreateTaskList(userId, taskList);

        if (taskListCreated == null)
        {
            return BadRequest();
        }

        return Ok(taskListCreated);
    }

    [HttpPost("CreateOneTask/{userId}/{taskListId}")]
    public IActionResult CreateTaskForUser(int userId, int taskListId, Task task)
    {
            
        var taskCreated = _userService.CreateTask(userId, taskListId, task);
        
        if (taskCreated != null)
        {
            return Ok(taskCreated);
        }
        
        return BadRequest();
    }

    [HttpDelete("RemoveTaskForUser/{userId}/{taskId}")]
    public IActionResult DeleteTaskForUser(int userId, int taskId)
    {
        var userRemoved= _userService.RemoveTask(userId, taskId);
     
        if (userRemoved)
        {
            return Ok();
        }
        
        return BadRequest();
    }

    [HttpPatch("EditTaskForUser/{userId}/{taskId}")]
    public IActionResult EditTaskForUser(int userId, int taskId, Task task)
    {
        var taskEdited = _userService.EditTask(userId, taskId, task);
        if (taskEdited)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpGet("GetStatusCounts/{userId}")]
    public IActionResult GetStausCounts(int userId)
    {
        var taskCounts = _userService.GetTaskStatusCounts(userId);
        if (taskCounts == null)
        {
            return BadRequest();
        }

        return Ok(new
        {
            notInProgress = taskCounts[0],
            inProgress = taskCounts[1],
            isComplete = taskCounts[2]
        });
    }
    
}