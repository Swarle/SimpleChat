using AutoMapper;
using SimpleChat.BL.DTO;
using SimpleChat.DAL.Entities;

namespace SimpleChat.BL.Helpers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Message, MessageDto>();
    }
}