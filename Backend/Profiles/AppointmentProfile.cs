using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using DomainData.DB.Entities;
using DTOs;

namespace Profiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<Appointment, AppointmentModel>().ReverseMap();
            CreateMap<AppointmentModel, AppointmentDTO>()
                .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.doctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ReverseMap();
        }
    }
}
