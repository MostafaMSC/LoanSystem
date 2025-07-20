// Models/LoginRequest.cs
namespace Real_State.Modules
{
    public class RegisterModule
    {
        public int Id {get;set;}
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string PhonNumber { get ; set ;}
        public string? Role { get; set; }

    }
}
