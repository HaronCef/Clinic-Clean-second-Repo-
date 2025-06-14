using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainData.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainData.DB
{
    public class ClinicContext : DbContext
    {

        DbSet<User> Users { get; set; }
        DbSet<Doctor> Doctors { get; set; }
        DbSet<Appointment> Appointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Port=5432;Database=ClinicDB;Username=postgres;Password=12345;Include Error Detail=true");
            options.EnableSensitiveDataLogging();
        }


    }
}
