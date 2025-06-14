using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services.Interfaces
{
    public interface IDoctorsService
    {
        public int CreateDoctor(DoctorModel doctor);
        public IEnumerable<DoctorModel> GetAll();
        public DoctorModel GetById(int id);
        public void Update(DoctorModel doctor);
        public void Delete(int id);
    }
}
