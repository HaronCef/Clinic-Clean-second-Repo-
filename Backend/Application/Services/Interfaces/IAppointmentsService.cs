using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services.Interfaces
{
    public interface IAppointmentsService
    {
        public IEnumerable<UserModel> GetUsersByDoctor(int doctorId);
        public IEnumerable<DoctorModel> GetDoctorsbyUser(int userId);
        public int CreateAppointmet(int doctorId, int userId);

    }
}
