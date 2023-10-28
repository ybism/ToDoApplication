namespace ToDoApplication.WebApi.Models.DTO;

public class ToDoRequestDTO
{
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAssociatedWithToDoList { get; set; }
        public Guid? ToDoListId { get; set; }
}