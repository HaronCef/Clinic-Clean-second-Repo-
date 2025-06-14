using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class AppointmentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }
    }
}
