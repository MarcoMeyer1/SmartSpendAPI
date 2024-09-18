namespace SmartSpend_API.Models
{
    public class User
    {
        public string FirstName { get; set; }    
        public string LastName { get; set; }     
        public string Email { get; set; }        
        public string Password { get; set; }    
        public string PhoneNumber { get; set; } = null;  // Optional
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
