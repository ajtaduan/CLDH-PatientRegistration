namespace CLDH.PatientRegistration.Models
{
    // Represent a staff login account for the system.
    // Only used for authentication, not related to patient data.

    public class User
    {
        public int Id { get; set; } // Primary key

        public string Username { get; set; } = string.Empty;

        // Never store the raw password, only the hashed version.
        // Hasing is one way, so even if the DB is leaked, passwords arent exposed.

        public string PasswordHash { get; set; } = string.Empty;
    }
}