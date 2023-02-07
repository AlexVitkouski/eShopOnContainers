namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

//using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

public record OrderStatusChangedToAwaitingCouponValidationIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }

    public string OrderStatus { get; }

    public int BuyerId { get; set; }

    public string BuyerName { get; }

    public string Code { get; }
    
    public double Points { get; }

    public OrderStatusChangedToAwaitingCouponValidationIntegrationEvent(int orderId, string orderStatus, string buyerName, string code, double points, int buyerId)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        Code = code;
        Points = points;
        BuyerId = buyerId;
    }
}

