using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Travalers.Enums;

namespace Travalers.Entities
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }

        public UserType UserType { get; set; }

        [BsonElement("NIC")]
        public string NIC { get; set; }

        [BsonElement("IsActive")]
        public bool IsActive { get; set; }


    }
}
