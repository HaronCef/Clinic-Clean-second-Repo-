using Application.Models;
using Application.Services;
using Application.Services.Interfaces;
using AutoMapper;
using ClinicAppWebApi.Controllers.Interfaces;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppWebApi.Controllers
{
    [ApiController]
    [Route("Doctors")]
    public class DoctorsController : ControllerBase, IDoctorsComtroller
    {
        private readonly IDoctorsService _doctorsService;
        private readonly IMapper _mapper;

        public DoctorsController(IDoctorsService doctorsService, IMapper mapper)
        {
            _doctorsService = doctorsService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var userModels = _doctorsService.GetAll();
            var result = userModels.Select(x => _mapper.Map<DoctorDTO>(x));
            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] DoctorDTO doctor) {
            var modelToCtreate = _mapper.Map<DoctorModel>(doctor);
            if(modelToCtreate != null)
            {
                var id =  _doctorsService.CreateDoctor(modelToCtreate);
                return Ok(id);
            }
            return BadRequest();
            
        }
        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            try
            {
                var doctorModel = _doctorsService.GetById(id);
                if (doctorModel != null)
                    return Ok(_mapper.Map<DoctorDTO>(doctorModel));
                return BadRequest("Лікаря не знайдено");
            }
            catch (Exception ex)
            {
                return BadRequest("Лікаря не знайдено");
            }
        }
        [HttpPut("Update")]
        public IActionResult UpdateDoctor([FromBody] DoctorDTO doctor)
        {
            var userModel = _mapper.Map<DoctorModel>(doctor);
            try
            {
                _doctorsService.Update(userModel);
            }
            catch (Exception ex)
            {
                return BadRequest("Лікаря не знайдено");
            }
            return Ok();
        }
        [HttpDelete("Delete{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var user = _doctorsService.GetById(id);
                if (user != null)
                {
                    _doctorsService.Delete(id);
                    return Ok();
                }
                return BadRequest("Лікаря не знайдено");
            }
            catch (Exception ex)
            {
                return BadRequest("Лікаря не знайдено");
            }
        }


    }
}
