namespace Travalers.DTOs.Ticket
{
    public class ReserveTicketDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string TrainId { get; set; }
        public int SeatNumber { get; set; }
        public int NoOfSeats { get; set; }
    }
}
