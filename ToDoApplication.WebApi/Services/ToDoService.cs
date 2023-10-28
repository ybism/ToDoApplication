using ToDoApplication.WebApi.Data;
using ToDoApplication.WebApi.Models;
using ToDoApplication.WebApi.Services.Interfaces;

namespace ToDoApplication.WebApi.Services;

public class ToDoService : IToDoService
{
    private readonly DatabaseContext _context;

    public ToDoService(DatabaseContext context)
    {
        _context = context;
    }

    public void CreateItem(ToDo toDo)
    {
        _context.ToDos.Add(toDo);
        _context.SaveChanges();
    }
    
    public bool AddNew(ToDo toDo)
    {
        if (_context.ToDos.Contains(toDo))
        {
            throw new Exception("Item already exists");
        }

        _context.ToDos.Add(toDo);
        _context.SaveChanges();
        return true;
    }

    public List<ToDo> GetAllToDo()
    {
        return _context.ToDos.ToList();
    }

    public ToDo GetToDo(Guid id)
    {
        ToDo toDoItem = _context.ToDos.FirstOrDefault(x => x.Id == id);
        if (toDoItem != null)
        {
            return toDoItem;
        }

        throw new Exception("Item does not exist");
    }

    public bool EditToDo(ToDo toDo)
    {
        if (!_context.ToDos.Contains(toDo))
        {
            throw new Exception("Item does not exist");
        }
        
        _context.ToDos.Update(toDo);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteToDo(Guid id)
    {
        var toDoItem = _context.ToDos.FirstOrDefault(x => x.Id == id);
        if (toDoItem == null)
        { 
            throw new Exception("Item does not exist");  
        }
        
        _context.ToDos.Remove(toDoItem);
        _context.SaveChanges();
        return true;
    }
}