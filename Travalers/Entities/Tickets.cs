using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Travalers.Entities
{
    public class Tickets
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("TrainId")]
        public string TrainId { get; set; }

        [BsonElement("SeatNumber")]
        public int SeatNumber { get; set; }

        [BsonElement("NoOfSeats")]
        public int NoOfSeats { get; set; }

        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }


    }
}
