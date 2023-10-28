using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.WebApi.Models;
using ToDoApplication.WebApi.Models.DTO;
using ToDoApplication.WebApi.Services.Interfaces;

namespace ToDoApplication.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _toDoService;
    private readonly IMapper _mappingProfile;

    public ToDoController(IToDoService toDoService, IMapper mappingProfile)
    {
        _toDoService = toDoService;
        _mappingProfile = mappingProfile;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToDoItem([FromBody] ToDoRequestDTO requestDto)
    {
        var toDoItem = _mappingProfile.Map<ToDo>(requestDto);
        
        var newItem = _toDoService.AddNew(toDoItem);
        if (newItem != null)
        {
            return Ok(_mappingProfile.Map<ToDoResponseDTO>(toDoItem));
        }

        return BadRequest("Unable to create the to-do item.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllToDoItems()
    {
        var items = _toDoService.GetAllToDo();
        
        var responseDTOs = items.Select(todo => _mappingProfile.Map<ToDoResponseDTO>(todo));
        
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingleToDo([FromRoute] Guid id)
    {
        var item = _toDoService.GetToDo(id);
        if (item == null)
        {
            return NotFound("Item does not exist");
        }

        var responseDTO = _mappingProfile.Map<ToDoResponseDTO>(item);
        return Ok(responseDTO);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateToDoItem([FromBody] ToDoRequestDTO requestDto)
    {
        var updatedItem = _toDoService.EditToDo(_mappingProfile.Map<ToDo>(requestDto));
        if (updatedItem != null)
        {
            var responseDTO = _mappingProfile.Map<ToDoResponseDTO>(updatedItem);
            return Ok(updatedItem);
        }

        return BadRequest("Unable to update the to-do item.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoItem([FromRoute] Guid id)
    {
        var deleted = _toDoService.DeleteToDo(id);

        if (deleted)
        {
            return NoContent();
        }

        return BadRequest("Unable to delete the to-do item.");
    }
}