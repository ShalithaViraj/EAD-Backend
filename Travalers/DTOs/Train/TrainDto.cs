using Travalers.Entities;

namespace Travalers.DTOs.Train
{
    public class TrainDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Discription { get; set; }
        public string? StartPoint { get; set; }
        public string? EndPoint { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Seats { get; set; }
        public List<Tickets> Tickets { get; set; }
    }
}
