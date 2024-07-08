using AutoMapper;
using SimpleChat.BL.DTO;
using SimpleChat.DAL.Entities;

namespace SimpleChat.BL.Helpers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Message, MessageDto>();
        CreateMap<Conversation, ConversationSearchDto>();
        CreateMap<UpdateConversationDto, Conversation>()
            .ForMember(dest => dest.Id, opt =>
                opt.Ignore())
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Member, MemberDto>()
            .ForMember(dest => dest.MemberRole, opt =>
                opt.MapFrom(src => src.UserRole.ToString()))
            .ForMember(dest => dest.Id, opt => 
                opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Name, opt => 
                opt.MapFrom(src => src.User.Name));

        CreateMap<Conversation, ConversationDto>();
    }
}