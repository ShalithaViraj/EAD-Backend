using Travalers.Enums;

namespace Travalers.DTOs.User
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserType UserType { get; set; }
        public string NIC { get; set; }
        public bool IsActive { get; set; }
    }
}
