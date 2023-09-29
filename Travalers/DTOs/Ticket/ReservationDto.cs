namespace Travalers.DTOs.Ticket
{
    public class ReservationDto
    {
        public string UserId { get; set; }
        public string TrainId { get; set; }
        public string UserName { get; set; }
        public string TrainName { get; set; }
        public int SeatNumber { get; set; }
    }
}
