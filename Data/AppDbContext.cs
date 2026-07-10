using Microsoft.EntityFrameworkCore;
using CLDH.PatientRegistration.Models;

namespace CLDH.PatientRegistration.Data
{
    // EF core's "bridge" between C# classes and actual sqlite database.
    // Each DbSet<T> below becomes a stable in the database

    public class AppDbContext : DbContext
    {
        // Passes connection settings (from Program.cs) up to the base DbContext class
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<Patient> Patients { get; set; } // becomes the "Patients" table
        public DbSet<User> Users { get; set; } // becomes the "Users" table
    }
}