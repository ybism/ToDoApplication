using Microsoft.AspNetCore.Components.Web;
using ToDoApplication.WebApi.Data;
using ToDoApplication.WebApi.Models;
using ToDoApplication.WebApi.Services.Interfaces;

namespace ToDoApplication.WebApi.Services;

public class ToDoListService : IToDoListService
{
    private readonly DatabaseContext _context;

    public ToDoListService(DatabaseContext context)
    {
        _context = context;
    }

    public List<ToDoList> GetUserToDoLists(Guid userId)
    {
        return _context.ToDoLists.Where(x => x.UserId == userId).ToList();
    }

    public ToDoList CreateUserToDoList(Guid userId, string title)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return null;
        }

        var toDoList = new ToDoList
        {
            Title = title,
            UserId = userId
        };

        _context.ToDoLists.Add(toDoList);
        _context.SaveChanges(); 

        return toDoList;
    }
    
    public ToDoList GetToDoList(Guid toDoListId)
    {
        return _context.ToDoLists.FirstOrDefault(tdl => tdl.Id == toDoListId);
    }

    public ToDoList UpdateToDoList(ToDoList toDoList)
    {
        _context.ToDoLists.Update(toDoList);
        _context.SaveChanges();
        return toDoList;
    }

    public void DeleteToDoList(Guid toDoListId)
    {
        var toDoList = _context.ToDoLists.FirstOrDefault(tdl => tdl.Id == toDoListId);
        if (toDoList != null)
        {
            _context.ToDoLists.Remove(toDoList);
            _context.SaveChanges();
        }

        throw new Exception("To Do List does not exist");
    }
    
    
}
