using ToDoApplication.WebApi.Models;

namespace ToDoApplication.WebApi.Services.Interfaces;

public interface IToDoService
{
    void CreateItem(ToDo toDo);
    bool AddNew(ToDo toDo);
    List<ToDo> GetAllToDo();
    ToDo GetToDo(Guid id);
    bool EditToDo(ToDo toDo);
    bool DeleteToDo(Guid id);
}