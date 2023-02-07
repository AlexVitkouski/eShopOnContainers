namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToValidatedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; set; }
    public string OrderStatus { get; set; }
    public string BuyerName { get; set; }

    public OrderStatusChangedToValidatedIntegrationEvent(int orderId, string orderStatus, string buyerName)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }
}
