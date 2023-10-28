using Microsoft.AspNetCore.Mvc;
using ToDoApplication.WebApi.Models;
using ToDoApplication.WebApi.Services.Interfaces;

namespace ToDoApplication.WebApi.Controllers;
   
[ApiController]
[Route("[controller]/ToDoList")]
public class ToDoListController : ControllerBase
{
    private readonly IToDoListService _toDoListService;

    public ToDoListController(IToDoListService toDoListService)
    {
        _toDoListService = toDoListService;
    }

    [HttpGet("User/{userId}")]
    public IActionResult GetUserToDoLists([FromRoute] Guid userId)
    {
        List<ToDoList> toDoLists = _toDoListService.GetUserToDoLists(userId);
        return Ok(toDoLists);
    }

    [HttpPost]
    public IActionResult CreateUserToDoList([FromBody] ToDoList model)
    {
        ToDoList toDoList = _toDoListService.CreateUserToDoList(model.UserId, model.Title);

        if (toDoList == null)
        {
            return BadRequest("User not found or unable to create to-do list.");
        }

        return Created($"api/todolists/{toDoList.Id}", toDoList);
    }

    [HttpGet("{toDoListId}")]
    public IActionResult GetToDoList([FromRoute] Guid toDoListId)
    {
        ToDoList toDoList = _toDoListService.GetToDoList(toDoListId);

        if (toDoList == null)
        {
            return NotFound("To-Do list not found.");
        }

        return Ok(toDoList);
    }

    [HttpPut("{toDoListId}")]
    public IActionResult UpdateToDoList([FromRoute] Guid toDoListId, [FromBody] ToDoList model)
    {
        ToDoList existingToDoList = _toDoListService.GetToDoList(toDoListId);

        if (existingToDoList == null)
        {
            return NotFound("To-Do list not found.");
        }

        existingToDoList.Title = model.Title;
        _toDoListService.UpdateToDoList(existingToDoList);

        return Ok(existingToDoList);
    }

    [HttpDelete("{toDoListId}")]
    public IActionResult DeleteToDoList([FromRoute] Guid toDoListId)
    {
        ToDoList existingToDoList = _toDoListService.GetToDoList(toDoListId);

        if (existingToDoList == null)
        {
            return NotFound("To-Do list not found.");
        }

        _toDoListService.DeleteToDoList(toDoListId);

        return NoContent();
    }
}