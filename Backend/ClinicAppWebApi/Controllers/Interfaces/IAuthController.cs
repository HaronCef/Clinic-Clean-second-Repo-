using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppWebApi.Controllers.Interfaces
{
    public interface IAuthController
    {
        public IActionResult Login([FromBody] AuthRequest request);
        public IActionResult Registration([FromBody] AuthRequest request);
    }
}
