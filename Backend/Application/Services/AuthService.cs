using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using DomainData.UoW;
using BCrypt;
using Application.Exceptions;
using BCrypt.Net;
using DomainData.DB.Entities;
using Application.Services.Interfaces;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public UserModel TryLogin(string username, string password)
        {
            var existingUser = _unitOfWork.UsersRepository.GetAll().FirstOrDefault(x => x.UserName == username);
            if (existingUser == null)
            {
                throw new UserNotFoundException();
            }
            
            else
            {
                var userModel = _mapper.Map<UserModel>(existingUser);
                bool isValid = BCrypt.Net.BCrypt.Verify(password, userModel.PasswordHash);
                if (isValid)
                {
                    return userModel;
                }
                else {
                    throw new UserNotFoundException();
                }
            }
        }
        public bool Registration(string username, string password)
        {
            var existingUser = _unitOfWork.UsersRepository.GetAll().FirstOrDefault(x => x.UserName == username);
            if(existingUser != null)
            {
                throw new UserAlreadyExistException();
            }
            else
            {
                
                var passwordHash = GetBCryptHash(password);
                _unitOfWork.UsersRepository.Create(new User { UserName = username, PasswordHash = passwordHash, Role = "user" });
                _unitOfWork.Save();
                return true;
            }
        }

        public string GetBCryptHash(string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }
    }
}
