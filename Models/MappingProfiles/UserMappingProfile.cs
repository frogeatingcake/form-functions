using AutoMapper;
using Form_Function_App.Models;
using Form_Function_App.Models.Dtos;

namespace Form_Function_App.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserTableDto>()
                .ForMember(dest => dest.PartitionKey, opt => opt.Ignore())
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => System.Guid.NewGuid().ToString()));

            CreateMap<UserTableDto, UserDto>();
            CreateMap<UserDto, User>(); // Mapping from UserDto to User

        }
    }
}
