using MongoDB.Driver;
using System.Net.Sockets;
using Travalers.Entities;

namespace Travalers.Repository
{
    public interface ITicketRepository
    {
        Task CreateTicketAsync(Tickets ticket);

        Task<Tickets> GetTicketByIdAsync(string id);

        Task<List<Tickets>> GetAllTicketsAsync();

        Task ReserveTicketAsync(string userId, int seatNumber);

        Task CancelTicketAsync(string id);
    }
}
