using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppWebApi.Controllers.Interfaces
{
    public interface IAppointmentController
    {
        public IActionResult GetUsersByDoctor([FromRoute] int id);
        public IActionResult GetDoctorsByUser([FromRoute] int id);
        public IActionResult CreateAppointment([FromBody] AppointmentReqeust appointment);

    }
}
