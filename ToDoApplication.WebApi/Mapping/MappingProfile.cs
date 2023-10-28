using AutoMapper;
using ToDoApplication.WebApi.Models;
using ToDoApplication.WebApi.Models.DTO;
using ToDoList = ToDoApplication.WebApi.Models.ToDoList;

namespace ToDoApplication.WebApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ToDoResponseDTO, ToDo>();
        
        CreateMap<ToDoRequestDTO, ToDo>()
            .ForMember(dest => dest.InProgress, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ToDoListId,
                opt => opt.MapFrom(src => src.IsAssociatedWithToDoList
                    ? src.ToDoListId
                    : null));

        CreateMap<ToDoListResponseDTO, ToDoList>();
        
    }
}