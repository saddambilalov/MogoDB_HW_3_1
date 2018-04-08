using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Models
{
    public class Score
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("score")]
        public double ScoreValue { get; set; }
    }
}
