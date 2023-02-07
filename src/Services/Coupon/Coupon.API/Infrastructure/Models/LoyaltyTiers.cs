using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Coupon.API.Infrastructure.Models
{
    public class LoyaltyTier
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Number { get; set; }

        public double Amount { get; set; }

        public double Discount { get; set; }
    }
}
