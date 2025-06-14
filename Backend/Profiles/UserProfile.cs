using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using DomainData.DB.Entities;
using DTOs;

namespace Profiles
{
    public class UserProfile : Profile
    {

        public UserProfile() {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<UserModel, UserDTO>()
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.UserName)).ReverseMap();

        }
    }
}
