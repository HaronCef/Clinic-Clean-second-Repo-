using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainData.DB.Entities
{
    public class Appointment
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }

    }
}
