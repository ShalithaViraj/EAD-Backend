using MongoDB.Driver;
using Travalers.Data;
using Travalers.Entities;

namespace Travalers.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Tickets> _ticketsCollection;

        public TicketRepository(IMongoDBContext dbContext)
        {
            _ticketsCollection = dbContext.Tickets;
        }

        public async Task CreateTicketAsync(Tickets ticket)
        {
            await _ticketsCollection.InsertOneAsync(ticket);
        }

        public async Task<Tickets> GetTicketByIdAsync(string id)
        {
            var filter = Builders<Tickets>.Filter.Eq(t => t.Id, id);
            return await _ticketsCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Tickets>> GetAllTicketsAsync()
        {
            return await _ticketsCollection.Find(_ => true).ToListAsync();
        }

        public async Task ReserveTicketAsync(string userId, int seatNumber)
        {
            var ticket = new Tickets
            {
                UserId = userId,
                SeatNumber = seatNumber
                // Other ticket properties can be set here
            };

            await CreateTicketAsync(ticket);
        }

        public async Task CancelTicketAsync(string id)
        {
            var filter = Builders<Tickets>.Filter.Eq(t => t.Id, id);
            await _ticketsCollection.DeleteOneAsync(filter);
        }
    }
}
