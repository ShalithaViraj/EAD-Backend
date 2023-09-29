using Travalers.Entities;

namespace Travalers.Repository
{
    public interface ITrainRepository
    {
        Task CreateTrainAsync(Train train);

        Task UpdateTrainAsync(Train train);

        Task<Train> GetTrainById(string id);

        Task<List<Train>> GetAllTrains();

        Task DeleteTrainAsync(string id);
    }
}
