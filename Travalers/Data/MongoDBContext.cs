using MongoDB.Driver;
using Travalers.Entities;

namespace Travalers.Data
{
    public class MongoDBContext : IMongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }

        public IMongoCollection<User> Users
        {
            get
            {
                return _database.GetCollection<User>("Users");
            }
        }

        public IMongoCollection<Train> Trains
        {
            get
            {
                return _database.GetCollection<Train>("Trains");
            }
        }

        public IMongoCollection<Tickets> Tickets
        {
            get
            {
                return _database.GetCollection<Tickets>("Tickets");
            }
        }
    }

}
