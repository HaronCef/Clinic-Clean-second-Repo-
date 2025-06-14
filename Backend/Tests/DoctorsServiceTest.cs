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
public class DoctorsServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<Doctor>> _mockDoctorsRepository;
    private readonly IMapper _mapper;
    private readonly DoctorsService _doctorsService;

    public DoctorsServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDoctorsRepository = new Mock<IGenericRepository<Doctor>>();
        _mockUnitOfWork.Setup(u => u.DoctorsRepository).Returns(_mockDoctorsRepository.Object);

        var config = new MapperConfiguration(cfg => {
            cfg.AddMaps(typeof(UserProfile).Assembly);
        });
        _mapper = config.CreateMapper();

        _doctorsService = new DoctorsService(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public void CreateDoctor_Should_CallCreate_And_Save_ReturnsId()
    {
        var doctorModel = new DoctorModel { Id = 0, Name = "Dr. Smith" };
        Doctor createdEntity = null;

        _mockDoctorsRepository.Setup(r => r.Create(It.IsAny<Doctor>()))
            .Callback<Doctor>(d => {
                d.Id = 42;
                createdEntity = d;
            });

        _mockUnitOfWork.Setup(u => u.Save());
 
        var resultId = _doctorsService.CreateDoctor(doctorModel);
 
        Assert.Equal(42, resultId);
        Assert.NotNull(createdEntity);
        Assert.Equal("Dr. Smith", createdEntity.Name);
        _mockDoctorsRepository.Verify(r => r.Create(It.IsAny<Doctor>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }

    [Fact]
    public void GetAll_ShouldReturn_AllDoctorsMapped()
    { 
        var doctors = new List<Doctor>
        {
            new Doctor { Id = 1, Name = "Dr. A" },
            new Doctor { Id = 2, Name = "Dr. B" }
        };

        _mockDoctorsRepository.Setup(r => r.GetAll()).Returns(doctors);

        var result = _doctorsService.GetAll().ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Name == "Dr. A");
        Assert.Contains(result, d => d.Name == "Dr. B");
    }

    [Fact]
    public void GetById_ExistingId_ReturnsMappedDoctor()
    {
        var doctorEntity = new Doctor { Id = 5, Name = "Dr. Who" };
        _mockDoctorsRepository.Setup(r => r.GetById(5)).Returns(doctorEntity);

        var result = _doctorsService.GetById(5);

        Assert.NotNull(result);
        Assert.Equal("Dr. Who", result.Name);
    }

    [Fact]
    public void GetById_NonExistingId_ThrowsUserNotFoundException()
    {
        _mockDoctorsRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new KeyNotFoundException());

        Assert.Throws<UserNotFoundException>(() => _doctorsService.GetById(999));
    }

    [Fact]
    public void Update_ExistingDoctor_MapsAndSaves()
    {
        var doctorModel = new DoctorModel { Id = 1, Name = "Updated Name" };
        var doctorEntity = new Doctor { Id = 1, Name = "Old Name" };

        _mockDoctorsRepository.Setup(r => r.GetTrackedOrAttach(1)).Returns(doctorEntity);
        _mockUnitOfWork.Setup(u => u.Save());

        _doctorsService.Update(doctorModel);

        Assert.Equal("Updated Name", doctorEntity.Name);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }

    [Fact]
    public void Update_NonExistingDoctor_ThrowsUserNotFoundException()
    {
        _mockDoctorsRepository.Setup(r => r.GetTrackedOrAttach(It.IsAny<int>())).Throws(new Exception());

        Assert.Throws<UserNotFoundException>(() => _doctorsService.Update(new DoctorModel { Id = 10 }));
    }

    [Fact]
    public void Delete_ExistingDoctor_DeletesAndSaves()
    {
        var doctorEntity = new Doctor { Id = 1, Name = "ToDelete" };
        _mockDoctorsRepository.Setup(r => r.GetTrackedOrAttach(1)).Returns(doctorEntity);
        _mockUnitOfWork.Setup(u => u.Save());

        _doctorsService.Delete(1);

        _mockDoctorsRepository.Verify(r => r.Delete(1), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }

    [Fact]
    public void Delete_NonExistingDoctor_ThrowsUserNotFoundException()
    {
        _mockDoctorsRepository.Setup(r => r.GetTrackedOrAttach(It.IsAny<int>())).Throws(new Exception());

        Assert.Throws<UserNotFoundException>(() => _doctorsService.Delete(123));
    }
}
