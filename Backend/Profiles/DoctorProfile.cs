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
    public class DoctorProfile : Profile
    {
        public DoctorProfile() {

            CreateMap<Doctor, DoctorModel>().ReverseMap();

            CreateMap<DoctorModel, DoctorDTO>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.specialization, opt => opt.MapFrom(src => src.Specialization.ToString()));

            CreateMap<DoctorDTO, DoctorModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => ParseSpecialization(src.specialization)));
        }
        private ESpecializations ParseSpecialization(string specialization)
        {
            return Enum.TryParse<ESpecializations>(specialization, true, out var result)
                ? result
                : ESpecializations.Therapist;
        }
    }
}
