namespace SmartSpend_API.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; } = null;  // Optional
        public DateTime DateRegistered { get; set; }    // Assuming you store the registration date
    }
    public class UserRegistrationModel
    {
        public string FirstName { get; set; }     // Required
        public string LastName { get; set; }      // Required
        public string Email { get; set; }         // Required
        public string Password { get; set; }      // Required
        public string? PhoneNumber { get; set; }  // Optional (nullable)
    }

    public class UserLoginModel
    {
        public string Email { get; set; }         // Required
        public string Password { get; set; }      // Required
    }

}
