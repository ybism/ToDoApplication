using ToDoApplication.WebApi.Models;

namespace ToDoApplication.WebApi.Services.Interfaces;

public interface IToDoListService
{
    List<ToDoList> GetUserToDoLists(Guid userId);
    ToDoList CreateUserToDoList(Guid userId, string title);
    ToDoList GetToDoList(Guid toDoListId);
    ToDoList UpdateToDoList(ToDoList toDoList);
    void DeleteToDoList(Guid toDoListId);
}