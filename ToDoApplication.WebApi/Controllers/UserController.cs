using Microsoft.AspNetCore.Mvc;
using ToDoApplication.WebApi.Models;
using ToDoApplication.WebApi.Services.Interfaces;

namespace ToDoApplication.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("createUser/{email}/{password}/{name}")]
    public async Task<IActionResult> CreateUser([FromRoute] string name, string email, string password)
    {
        User userToCreate = _userService.RegisterUser(name, email, password);

        if (userToCreate == null)
        {
            return BadRequest();
        }

        return Ok(userToCreate);
    }
    
    [HttpGet("profile")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        User userToReturn = _userService.GetUser(userId);

        if (userId == null)
        {
            return BadRequest();
        }

        return Ok(userToReturn);
    }
    
    [HttpGet("login/{email}/{password}")]
    public async Task<IActionResult> LoginUser([FromRoute] string email, string password)
    {
        bool userExists = _userService.AuthenticateUser(email, password);

        if (!userExists)
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpGet("{userId}/todolists")]
    public async Task<ActionResult<IEnumerable<ToDoList>>> GetToDoListsForUser(Guid userId)
    {
        var user = await _userService.GetToDoListsForUsers(userId);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var toDoLists = user.ToDoLists;
        
        if (toDoLists.Count == 0)
        {
            return NotFound("user has no lists");
        }
        
        return Ok(toDoLists);
    }

    [HttpGet("{userId}/{listId}/todos")]
    public async Task<ActionResult<IEnumerable<ToDo>>> GetToDosForUserAndList([FromRoute] Guid userId, Guid listId)
    {
        var toDoItems = await _userService.GetToDoItemsForUser(userId);

        if (toDoItems == null)
        {
            return NotFound("User Not Found");
        }

        var toDoList = toDoItems.FirstOrDefault(tdl => tdl.Id == listId);

        if (toDoList == null)
        {
            return NotFound("To-do list not found");
        }

        return Ok(toDoList);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadProfilePicture(Guid userId, IFormFile profilePicture)
    {
        if (profilePicture == null)
        {
            return BadRequest("No image");
        }

        var result = _userService.SetUserProfilePhoto(userId, profilePicture);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetProfilePicture(Guid userId)
    {
        var result = _userService.GetUserProfilePhoto(userId);

        if (result == null)
        {
            return BadRequest("Image doesnt exist or not found");
        }

        return Ok(result);
    }
}