using Application.Models;
using Application.Services.Interfaces;
using AutoMapper;
using ClinicAppWebApi.Controllers.Interfaces;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppWebApi.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UsersController : ControllerBase, IUsersController
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(IUsersService usersService, IMapper mapper)
        {
            _mapper = mapper;
            _usersService = usersService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() {
            var userModels = _usersService.GetAll();
            var result = userModels.Select(x => _mapper.Map<UserDTO>(x));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetbyId(int id) {
            try
            {
                var userModel = _usersService.GetById(id);
                if (userModel != null)
                    return Ok(_mapper.Map<UserDTO>(userModel));
                return BadRequest("Користувача не знайдено");
            }
            catch (Exception ex)
            {
                return BadRequest("Користувача не знайдено");
            }
        }
        [HttpPut("Update")]
        public IActionResult UpdateUser([FromBody] UserDTO user) {
            var userModel = _mapper.Map<UserModel>(user);
            try
            {
                _usersService.Update(userModel);
            }
            catch (Exception ex) {
                return BadRequest("Користувача не знайдено");
            }
            return Ok();
        }
        [HttpPut("MakeManager{id}")]
        public IActionResult MakeManager([FromRoute]int id)
        {
            var userModel = _usersService.GetById(id);
            userModel.Role = "manager";
            _usersService.Update(userModel);
            return Ok();
        }
        [HttpDelete("Delete{id}")]
        public IActionResult Delete([FromRoute] int id) {
            try
            {
                var user = _usersService.GetById(id);
                if (user != null)
                {
                    _usersService.Delete(id);
                    return Ok();
                }
                return BadRequest("Користувача не знайдено");
            }
            catch (Exception ex) {
                return BadRequest("Користувача не знайдено");
            }
        }

    }
}
