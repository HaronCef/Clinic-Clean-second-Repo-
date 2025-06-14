using Application.Exceptions;
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
    [Route("Auth")]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            UserModel user;
            if(request.email == "" || request.password == "")
            {
                return BadRequest("Поля логіну та паролю не можуть бути пустими");
            }
            else
            {
                try
                {
                   user = _authService.TryLogin(request.email, request.password);
                }
                catch (UserNotFoundException ex) {
                    return BadRequest("Неправильний логін чи пароль");
                }
                var result = _mapper.Map<UserDTO>(user);
                return Ok(result);
            }
        }
        [HttpPost("Registration")]
        public IActionResult Registration([FromBody] AuthRequest request)
        {
            if (request.email == "" || request.password == "")
            {
                return BadRequest("Поля логіну та паролю не можуть бути пустими");
            }
            bool state;
            try
            {
                state = _authService.Registration(request.email, request.password);
                if (state)
                {

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(UserAlreadyExistException ex)
            {
                return BadRequest("Користувач з таким юзернеймом вже існує");
            }
        }
    }
}
