using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class AppointmentAlreadyExistException : Exception
    {
        public AppointmentAlreadyExistException()
        {
            
        }
        public AppointmentAlreadyExistException(string message) : base(message) { }
    }
}
