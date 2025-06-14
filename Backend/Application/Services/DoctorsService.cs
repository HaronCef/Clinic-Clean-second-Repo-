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
    public class DoctorsService : IDoctorsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DoctorsService(IUnitOfWork unitOfWork, IMapper mapper) { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public int CreateDoctor(DoctorModel doctor)
        {
            var entityToCreate = _mapper.Map<Doctor>(doctor);
            _unitOfWork.DoctorsRepository.Create(entityToCreate);
            _unitOfWork.Save();
            return entityToCreate.Id;

        }
        
        public void Update(DoctorModel doctor)
        {
            try
            {
                var entityToUpdate = _unitOfWork.DoctorsRepository.GetTrackedOrAttach(doctor.Id);
                if (entityToUpdate != null)
                {
                    _mapper.Map(doctor, entityToUpdate);
                    _unitOfWork.Save();
                }
            }
            catch
            {
                throw new UserNotFoundException();
            }

        }
        public IEnumerable<DoctorModel> GetAll()
        {
            var doctorEntities = _unitOfWork.DoctorsRepository.GetAll();
            var result = doctorEntities.Select(x => { return _mapper.Map<DoctorModel>(x); });
            return result;
        }
        public DoctorModel GetById(int id)
        {
            try
            {
                var result = _mapper.Map<DoctorModel>(_unitOfWork.DoctorsRepository.GetById(id));
                return result;

            }
            catch (Exception e)
            {
                throw new UserNotFoundException();
            }
        }
        public void Delete(int id)
        {
            try
            {
                var entity = _unitOfWork.DoctorsRepository.GetTrackedOrAttach(id);
                if (entity != null)
                    _unitOfWork.DoctorsRepository.Delete(id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new UserNotFoundException();
            }
        }

    }
}
