using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppWebApi.Controllers.Interfaces
{
    public interface IDoctorsComtroller
    {
        public IActionResult GetAll();
        public IActionResult Create([FromBody] DoctorDTO doctor);
        public IActionResult GetbyId(int id);
        public IActionResult UpdateDoctor([FromBody] DoctorDTO doctor);
        public IActionResult Delete([FromRoute] int id);
    }
}
