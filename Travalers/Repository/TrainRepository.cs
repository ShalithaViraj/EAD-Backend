using MongoDB.Driver;
using Travalers.Data;
using Travalers.Entities;

namespace Travalers.Repository
{
    public class TrainRepository : ITrainRepository
    {
        private readonly IMongoCollection<Train> _trainsCollection;
        public TrainRepository(IMongoDBContext dbContext)
        {
            _trainsCollection = dbContext.Trains;
        }

        public async Task CreateTrainAsync(Train train)
        {
            await _trainsCollection.InsertOneAsync(train);
        }

        public async Task UpdateTrainAsync(Train train)
        {
            var filter = Builders<Train>.Filter.Eq(t => t.Id, train.Id);
            var options = new ReplaceOptions { IsUpsert = false };
            await _trainsCollection.ReplaceOneAsync(filter, train, options);
        }

        public async Task<Train> GetTrainById(string id)
        {
            var filter = Builders<Train>.Filter.Eq(t => t.Id, id);
            return await _trainsCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Train>> GetAllTrains()
        {
            return await _trainsCollection.Find(_ => true).ToListAsync();
        }

        public async Task DeleteTrainAsync(string id)
        {
            var filter = Builders<Train>.Filter.Eq(t => t.Id, id);
            var result = await _trainsCollection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception("Train not found or could not be deleted.");
            }
        }

    }
}

