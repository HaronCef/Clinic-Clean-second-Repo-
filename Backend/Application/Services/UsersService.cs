using Application.Exceptions;
using Application.Models;
using Application.Services.Interfaces;
using AutoMapper;
using DomainData.UoW;


namespace Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UsersService(IUnitOfWork UoW, IMapper mapper) {
            _unitOfWork = UoW;
            _mapper = mapper;
        }

        public IEnumerable<UserModel> GetAll()
        {
            var result = _unitOfWork.UsersRepository.GetAll().Select(x => _mapper.Map<UserModel>(x));
            return result;
        }
        public UserModel GetById(int id) {
            try
            {
                var result = _mapper.Map<UserModel>(_unitOfWork.UsersRepository.GetById(id));
                return result;

            }
            catch (Exception e) {
                throw new UserNotFoundException();
            }
        }
        public void Update(UserModel user) {
            try
            {
                var entityToUpdate = _unitOfWork.UsersRepository.GetTrackedOrAttach(user.Id);
                if (entityToUpdate != null)
                {
                    _mapper.Map(user, entityToUpdate);
                    _unitOfWork.Save();
                }
            }
            catch {
            
            }
            
        }
        public void Delete(int id) {
            try
            {
                var entity = _unitOfWork.UsersRepository.GetTrackedOrAttach(id);
                if (entity != null)
                    _unitOfWork.UsersRepository.Delete(id);
                _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                throw new UserNotFoundException();
            }
        }
        

    }
}
