using Application.Models;
using Application.Services.Interfaces;
using AutoMapper;
using ClinicAppWebApi.Controllers.Interfaces;
using Contracts;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppWebApi.Controllers
{
    [ApiController]
    [Route("Appointments")]
    public class AppointmentController : ControllerBase, IAppointmentController
    {
        private readonly IAppointmentsService _appointmentsService;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentsService appointmentsService, IMapper mapper)
        {
            _appointmentsService = appointmentsService;
            _mapper = mapper;
        }
        [HttpGet("GetUsersByDoctor{id}")]
        public IActionResult GetUsersByDoctor([FromRoute]int id)
        {
            try
            {
                var usersModels = _appointmentsService.GetUsersByDoctor(id);
                var result = usersModels.Select(x => _mapper.Map<UserDTO>(x));
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest("Лікаря не знайдено");
            }
        }
        [HttpGet("GetDoctorsByUser{id}")]
        public IActionResult GetDoctorsByUser([FromRoute] int id)
        {
            try
            {
                var doctorModels = _appointmentsService.GetDoctorsbyUser(id);
                return Ok(doctorModels.Select(x => _mapper.Map<DoctorDTO>(x)));
            }
            catch (Exception ex)
            {
                return BadRequest("Користувача не знайдено");
            }
        }
        [HttpPost("Create")]
        public IActionResult CreateAppointment([FromBody] AppointmentReqeust appointment)
        {
            try
            {
               return Ok(_appointmentsService.CreateAppointmet(appointment.doctorId, appointment.userId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
