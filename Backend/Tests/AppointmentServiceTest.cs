using System.Linq.Expressions;
using Application.Exceptions;
using Application.Services;
using AutoMapper;
using DomainData.DB.Entities;
using DomainData.Repository;
using DomainData.UoW;
using Moq;
using Profiles;

namespace Tests
{
    public class AppointmentsServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Appointment>> _mockAppointmentsRepository;
        private readonly Mock<IGenericRepository<User>> _mockUsersRepository;
        private readonly Mock<IGenericRepository<Doctor>> _mockDoctorsRepository;
        private readonly AppointmentsService _appointmentsService;

        public AppointmentsServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAppointmentsRepository = new Mock<IGenericRepository<Appointment>>();
            _mockUsersRepository = new Mock<IGenericRepository<User>>();
            _mockDoctorsRepository = new Mock<IGenericRepository<Doctor>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(UserProfile).Assembly); 
            });

            var mapper = config.CreateMapper();

            _mockUnitOfWork.Setup(x => x.AppointmentsRepository).Returns(_mockAppointmentsRepository.Object);
            _mockUnitOfWork.Setup(x => x.UsersRepository).Returns(_mockUsersRepository.Object);
            _mockUnitOfWork.Setup(x => x.DoctorsRepository).Returns(_mockDoctorsRepository.Object);

            _appointmentsService = new AppointmentsService(_mockUnitOfWork.Object, mapper);
        }

        [Fact]
        public void GetUsersByDoctor_ReturnsMappedUsers()
        {
            int doctorId = 1;
            var appointments = new List<Appointment>
            {
            new Appointment { DoctorId = doctorId, UserId = 10 },
            new Appointment { DoctorId = doctorId, UserId = 20 }
            };
            var users = new List<User>
            {
            new User { Id = 10, UserName = "Alice" },
            new User { Id = 20, UserName = "Bob" }
            };

            _mockAppointmentsRepository.Setup(r => r.GetAll()).Returns(appointments);
            _mockUsersRepository.Setup(r => r.GetById(10)).Returns(users[0]);
            _mockUsersRepository.Setup(r => r.GetById(20)).Returns(users[1]);

            var result = _appointmentsService.GetUsersByDoctor(doctorId).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Id == 10 && r.UserName == "Alice");
            Assert.Contains(result, r => r.Id == 20 && r.UserName == "Bob");
        }

        [Fact]
        public void GetDoctorsByUser_ReturnsMappedDoctors()
        {
            int userId = 5;
            var appointments = new List<Appointment>
            {
                new Appointment { UserId = userId, DoctorId = 100 },
                new Appointment { UserId = userId, DoctorId = 200 }
            };
            var doctors = new List<Doctor>
            {
                new Doctor { Id = 100, Name = "Dr. House" },
                new Doctor { Id = 200, Name = "Dr. Strange" }
            };

            _mockAppointmentsRepository.Setup(r => r.GetAll()).Returns(appointments);
            _mockDoctorsRepository.Setup(r => r.GetById(100)).Returns(doctors[0]);
            _mockDoctorsRepository.Setup(r => r.GetById(200)).Returns(doctors[1]);

            
            var result = _appointmentsService.GetDoctorsbyUser(userId).ToList();

            
            Assert.Equal(2, result.Count);
            Assert.Contains(result, d => d.Id == 100 && d.Name == "Dr. House");
            Assert.Contains(result, d => d.Id == 200 && d.Name == "Dr. Strange");
        }

        [Fact]
        public void CreateAppointment_ThrowsException_IfExists()
        {
             
            int doctorId = 1, userId = 2;
            _mockAppointmentsRepository
                .Setup(r => r.GetByFilter(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<Func<IQueryable<Appointment>, IQueryable<Appointment>>[]>()))
                .Returns(new List<Appointment> { new Appointment { DoctorId = doctorId, UserId = userId } });

            
            Assert.Throws<AppointmentAlreadyExistException>(() =>
                _appointmentsService.CreateAppointmet(doctorId, userId));
        }

        [Fact]
        public void CreateAppointment_CreatesNew_IfNotExists()
        {
            
            int doctorId = 3, userId = 4;
            Appointment created = null;

            _mockAppointmentsRepository
                .Setup(r => r.GetByFilter(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<Func<IQueryable<Appointment>, IQueryable<Appointment>>[]>()))
                .Returns(Enumerable.Empty<Appointment>().AsQueryable());

            _mockAppointmentsRepository
                .Setup(r => r.Create(It.IsAny<Appointment>()))
                .Callback<Appointment>(a =>
                {
                    a.Id = 99;
                    created = a;
                });

            _mockUnitOfWork.Setup(u => u.Save());

            
            var resultId = _appointmentsService.CreateAppointmet(doctorId, userId);

             
            Assert.Equal(99, resultId);
            Assert.NotNull(created);
            Assert.Equal(doctorId, created.DoctorId);
            Assert.Equal(userId, created.UserId);
        }
    }
}
