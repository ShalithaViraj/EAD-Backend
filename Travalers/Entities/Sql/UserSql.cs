using Travalers.Enums;

namespace Travalers.Entities.Sql
{
    public class UserSql
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public UserType UserType { get; set; }
        public string NIC { get; set; }
        public bool IsActive { get; set; }
    }
}
