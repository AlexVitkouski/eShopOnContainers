using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Coupon.API.Infrastructure.Models
{
    public class Loyalty
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int BuyerId { get; set; }

        public double Points { get; set; }

        public double TotalAmount { get; set; }
    }
}
