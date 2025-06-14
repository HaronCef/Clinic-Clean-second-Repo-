using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppWebApi.Controllers.Interfaces
{
    public interface IUsersController
    {
        public IActionResult GetAll();
        public IActionResult GetbyId(int id);
        public IActionResult UpdateUser([FromBody] UserDTO user);
        public IActionResult Delete([FromRoute] int id);


    }
}
