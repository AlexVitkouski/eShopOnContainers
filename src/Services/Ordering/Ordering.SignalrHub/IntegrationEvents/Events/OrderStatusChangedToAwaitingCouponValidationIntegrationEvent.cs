using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Newtonsoft.Json;

namespace Microsoft.eShopOnContainers.Services.Ordering.SignalrHub.IntegrationEvents;

public record OrderStatusChangedToAwaitingCouponValidationIntegrationEvent : IntegrationEvent
{
    [JsonProperty]
    public int OrderId { get; private set; }

    [JsonProperty]
    public string OrderStatus { get; private set; }

    [JsonProperty]
    public int BuyerId { get; set; }

    [JsonProperty]
    public string BuyerName { get; private set; }

    [JsonProperty]
    public string Code { get; private set; }

    [JsonProperty] 
    public double Points { get; set; }
}

