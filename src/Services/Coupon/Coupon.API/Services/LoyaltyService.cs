using Coupon.API.Infrastructure.Models;
using Coupon.API.Infrastructure.Repositories;
using Coupon.API.Services.Contract;
using OrderItem = Coupon.API.IntegrationEvents.Events.OrderItem;

namespace Coupon.API.Services
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly ICouponRepository _couponRepository;

        const decimal PointPercent = 0.07m;

        public LoyaltyService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task SaveLoyalty(int buyerId, List<OrderItem> orderItems)
        {
            var oldLoyalty = await _couponRepository.FindLoyaltyByCodeAsync(buyerId);
            
            var orderAmount = GetOrderAmount(orderItems);
            var orderPoints = GetOrderPoints(orderAmount);

            var newLoyalty = new Loyalty()
            {
                BuyerId = buyerId,
                Points = (oldLoyalty?.Points ?? 0) + orderPoints,
                TotalAmount = (oldLoyalty?.TotalAmount ?? 0) + orderAmount
            };

            await _couponRepository.SaveLoyalty(newLoyalty);
        }

        private decimal GetOrderAmount(List<OrderItem> orderItems)
        {
            return orderItems.Sum(i => i.UnitPrice * i.UnitCount);
        }

        private int GetOrderPoints(decimal orderAmount)
        {
            return (int)Math.Round(orderAmount*PointPercent);
        }
    }
}
