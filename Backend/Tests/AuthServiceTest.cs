using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Application.Services;
using Application.Models;
using DomainData.DB.Entities;
using DomainData.UoW;
using Application.Exceptions;
using DomainData.Repository;
using Profiles;

namespace Tests;
public class AuthServiceTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<User>> _mockUsersRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;

    public AuthServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUsersRepository = new Mock<IGenericRepository<User>>();

        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(UserProfile).Assembly));
        _mapper = config.CreateMapper();

        _mockUnitOfWork.Setup(u => u.UsersRepository).Returns(_mockUsersRepository.Object);

        _authService = new AuthService(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public void TryLogin_ValidCredentials_ReturnsUserModel()
    { 
        var password = "securePassword";
        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User { Id = 1, UserName = "test", PasswordHash = hash };
        _mockUsersRepository.Setup(r => r.GetAll()).Returns(new List<User>{ user });
 
        var result = _authService.TryLogin("test", password);
 
        Assert.Equal(user.UserName, result.UserName);
    }

    [Fact]
    public void TryLogin_InvalidPassword_ThrowsUserNotFoundException()
    {
        var user = new User { Id = 1, UserName = "test", PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct") };
        _mockUsersRepository.Setup(r => r.GetAll()).Returns(new List<User> { user });

        Assert.Throws<UserNotFoundException>(() => _authService.TryLogin("test", "wrong"));
    }

    [Fact]
    public void TryLogin_UserNotFound_ThrowsUserNotFoundException()
    {
        _mockUsersRepository.Setup(r => r.GetAll()).Returns(new List<User>());

        Assert.Throws<UserNotFoundException>(() => _authService.TryLogin("unknown", "password"));
    }

    [Fact]
    public void Registration_NewUser_ReturnsTrue()
    {
        User createdUser = null;

        _mockUsersRepository
            .Setup(r => r.GetAll())
            .Returns(new List<User>());

        _mockUsersRepository
            .Setup(r => r.Create(It.IsAny<User>()))
            .Callback<User>(u => createdUser = u);

        bool result = _authService.Registration("newUser", "1234");

        Assert.True(result);
        Assert.NotNull(createdUser);
        Assert.Equal("newUser", createdUser.UserName);
        Assert.True(BCrypt.Net.BCrypt.Verify("1234", createdUser.PasswordHash));
        _mockUsersRepository.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void Registration_UserAlreadyExists_ThrowsUserAlreadyExistException()
    {
        var existingUser = new User { UserName = "existing" };
        _mockUsersRepository.Setup(r => r.GetAll()).Returns(new List<User> { existingUser });

        Assert.Throws<UserAlreadyExistException>(() => _authService.Registration("existing", "1234"));
    }
}
