﻿namespace Ordering.API.Application.IntegrationEvents.Events
{
    public record BuyerPointsUpdatedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public int BuyerId { get; }

        public List<OrderItem> OrderItems { get; }

        public BuyerPointsUpdatedIntegrationEvent(int orderId, int buyerId, List<OrderItem> orderItems)
        {
            OrderId = orderId;
            BuyerId = buyerId;
            OrderItems = orderItems;
        }
    }

    public record OrderItem
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCount { get; set; }
    }
}