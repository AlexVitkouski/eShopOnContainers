using Coupon.API.Infrastructure.Models;
using OrderItem = Coupon.API.IntegrationEvents.Events.OrderItem;

namespace Coupon.API.Services.Contract
{
    public interface ILoyaltyService
    {
        Task SaveLoyalty(int buyerId, List<OrderItem> orderItems);
    }
}
