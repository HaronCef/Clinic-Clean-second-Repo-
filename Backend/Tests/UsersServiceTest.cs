using Xunit;
using Moq;
using AutoMapper;
using Application.Services;
using Application.Models;
using DomainData.DB.Entities;
using DomainData.UoW;
using System.Collections.Generic;
using System.Linq;
using Application.Exceptions;
using DomainData.Repository;
using Profiles;

namespace Tests;
public class UsersServiceTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<User>> _mockUsersRepository;
    private readonly UsersService _usersService;
    private readonly IMapper _mapper;

    public UsersServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUsersRepository = new Mock<IGenericRepository<User>>();

        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(UserProfile).Assembly));
        _mapper = config.CreateMapper();

        _mockUnitOfWork.Setup(u => u.UsersRepository).Returns(_mockUsersRepository.Object);

        _usersService = new UsersService(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public void GetAll_ReturnsMappedUsers()
    {
        var users = new List<User>
        {
            new User { Id = 1, UserName = "Test 1" },
            new User { Id = 2, UserName = "Test 2" }
        };

        _mockUsersRepository.Setup(r => r.GetAll()).Returns(users);

        var result = _usersService.GetAll();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, u => u.Id == 1);
    }

    [Fact]
    public void GetById_ExistingId_ReturnsUserModel()
    {
        var user = new User { Id = 1, UserName = "Test" };
        _mockUsersRepository.Setup(r => r.GetById(1)).Returns(user);

        var result = _usersService.GetById(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void GetById_NonExistingId_ThrowsUserNotFoundException()
    {
        _mockUsersRepository.Setup(r => r.GetById(1)).Throws<Exception>();

        Assert.Throws<UserNotFoundException>(() => _usersService.GetById(1));
    }

    [Fact]
    public void Update_ValidUser_UpdatesAndSaves()
    {
        var userModel = new UserModel { Id = 1, UserName = "Updated" };
        var userEntity = new User { Id = 1, UserName = "Old" };

        _mockUsersRepository.Setup(r => r.GetTrackedOrAttach(1)).Returns(userEntity);

        _usersService.Update(userModel);

        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        Assert.Equal("Updated", userEntity.UserName);
    }

    [Fact]
    public void Delete_ValidId_DeletesAndSaves()
    {
        var user = new User { Id = 1 };
        _mockUsersRepository.Setup(r => r.GetTrackedOrAttach(1)).Returns(user);

        _usersService.Delete(1);

        _mockUsersRepository.Verify(r => r.Delete(1), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }

    [Fact]
    public void Delete_InvalidId_ThrowsUserNotFoundException()
    {
        _mockUsersRepository.Setup(r => r.GetTrackedOrAttach(1)).Throws<Exception>();

        Assert.Throws<UserNotFoundException>(() => _usersService.Delete(1));
    }
}
