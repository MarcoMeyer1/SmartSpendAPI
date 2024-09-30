namespace SmartSpend_API.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; } = null;  
        public DateTime DateRegistered { get; set; }    
    }
    public class UserRegistrationModel
    {
        public string FirstName { get; set; }     
        public string LastName { get; set; }      
        public string Email { get; set; }         
        public string Password { get; set; }      
        public string? PhoneNumber { get; set; }  
    }

    public class UserLoginModel
    {
        public string Email { get; set; }         
        public string Password { get; set; }     
    }

}
