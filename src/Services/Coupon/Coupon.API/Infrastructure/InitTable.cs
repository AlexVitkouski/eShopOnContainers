using Coupon.API.Infrastructure.Models;
using Coupon.API.Infrastructure.Repositories;

namespace Coupon.API.Infrastructure
{
    public class DbInitService
    {
        public async Task InitTiers(CouponContext context)
        {
            if (context.LoyaltyTiers.EstimatedDocumentCount() == 0)
            {
                var loyaltyTiers = new List<LoyaltyTier>
                {
                    new LoyaltyTier()
                    {
                        Number = 1,
                        Amount = 100,
                        Discount = 1
                    },
                    new LoyaltyTier()
                    {
                        Number = 1,
                        Amount = 200,
                        Discount = 2
                    },
                    new LoyaltyTier()
                    {
                        Number = 1,
                        Amount = 300,
                        Discount = 3
                    }
                };

                await context.LoyaltyTiers.InsertManyAsync(loyaltyTiers);
            }
        }
    }

}
