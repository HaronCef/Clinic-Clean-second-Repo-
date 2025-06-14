using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class AppointmentDTO
    {
        public int id {  get; set; }
        public int userId { get; set; }
        public int doctorId { get; set; }
    }
}
