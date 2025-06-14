using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainData.DB.Entities;
using DomainData.Repository;

namespace DomainData.UoW
{
    public interface IUnitOfWork
    {
        public IGenericRepository<User> UsersRepository { get; }
        public IGenericRepository<Doctor> DoctorsRepository { get; }
        public IGenericRepository<Appointment> AppointmentsRepository { get; }
        public void Save();
    }
}
