using MongoDB.Driver;
using Travalers.Entities;

namespace Travalers.Data
{
    public interface IMongoDBContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Train> Trains { get; }
        IMongoCollection<Tickets> Tickets { get; }
    }
}
