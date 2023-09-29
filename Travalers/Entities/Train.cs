using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Travalers.Entities
{
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("Price")]
        public string? Price { get; set; }

        [BsonElement("Discription")]
        public string? Discription { get; set; }

        [BsonElement("StartPoint")]
        public string? StartPoint { get; set; }

        [BsonElement("EndPoint")]
        public string? EndPoint { get; set; }

        [BsonElement("StartTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("EndTime")]
        public DateTime EndTime { get; set; }

        [BsonElement("Seats")]
        public int Seats { get; set; }

        [BsonElement("Tickets")]
        public List<Tickets> Tickets { get; set; }


    }
}
