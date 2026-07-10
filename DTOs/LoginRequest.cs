namespace CLDH.PatientRegistration.DTOs
{
    // Shape of the data the frontend sends when logging in
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}