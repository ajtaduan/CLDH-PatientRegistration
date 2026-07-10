using System.ComponentModel.DataAnnotations;

namespace CLDH.PatientRegistration.Models
{
    // Represent a single patient record in the system.
    // Maps directly to the "Patients" table in Sqlite via EF core.

    public class Patient
    { 
        public int Id { get; set; } // Primary key, auto-incremented by EF core

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ContactNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;
    }
}