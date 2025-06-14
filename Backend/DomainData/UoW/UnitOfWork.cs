using DomainData.DB;
using DomainData.DB.Entities;
using DomainData.Repository;

namespace DomainData.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposedValue;

        private readonly ClinicContext clinicContext;
        private IGenericRepository<User> _usersRepository;
        private IGenericRepository<Doctor> _doctorsRepository;
        private IGenericRepository<Appointment> _appointmetsRepository;

        public IGenericRepository<User> UsersRepository => _usersRepository ??= new GenericRepository<User>(clinicContext);
        public IGenericRepository<Doctor> DoctorsRepository => _doctorsRepository ??= new GenericRepository<Doctor>(clinicContext);
        public IGenericRepository<Appointment> AppointmentsRepository => _appointmetsRepository ??= new GenericRepository<Appointment>(clinicContext);


        public UnitOfWork(ClinicContext context)
        {
            this.clinicContext = context;
        }

        public void Save()
        {
            clinicContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    clinicContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
