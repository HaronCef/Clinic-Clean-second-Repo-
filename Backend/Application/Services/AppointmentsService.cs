using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Models;
using Application.Services.Interfaces;
using AutoMapper;
using DomainData.DB.Entities;
using DomainData.UoW;

namespace Application.Services
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        

        public IEnumerable<UserModel> GetUsersByDoctor(int doctorId)
        {
            var usersList = _unitOfWork.AppointmentsRepository.GetAll().Where(x => x.DoctorId == doctorId).Select(x => { return _unitOfWork.UsersRepository.GetById(x.UserId); });
            return usersList.Select(x => { return _mapper.Map<UserModel>(x); });
        }
        public IEnumerable<DoctorModel> GetDoctorsbyUser(int userId)
        {
            var doctorIdList = _unitOfWork.AppointmentsRepository.GetAll().Where(x => x.UserId == userId).Select(x => { return _unitOfWork.DoctorsRepository.GetById(x.DoctorId); });
            return doctorIdList.Select(x => { return _mapper.Map<DoctorModel>(x); });
        }
        public int CreateAppointmet(int doctorId, int userId)
        {
            try
            {
                var existAppointment = _unitOfWork.AppointmentsRepository.GetByFilter(x => x.UserId == userId && x.DoctorId == doctorId);
                if (existAppointment.Count() > 0)
                    throw new AppointmentAlreadyExistException();
            }
            catch (AppointmentAlreadyExistException ex)
            {
                throw new AppointmentAlreadyExistException();
            }
            catch (Exception ex) { }
            var entityToCreate = new Appointment() { DoctorId = doctorId, UserId = userId };
            _unitOfWork.AppointmentsRepository.Create(entityToCreate);
            _unitOfWork.Save();

            return entityToCreate.Id;
        }
    }
}
