using Coupon.API.Infrastructure.Models;
using Coupon.API.Infrastructure.Repositories;
using Coupon.API.IntegrationEvents.Events;
using Coupon.API.Services.Contract;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Serilog.Context;

namespace Coupon.API.IntegrationEvents.EventHandlers
{

    public class BuyerPointsUpdatedIntegrationEventHandler :
        IIntegrationEventHandler<BuyerPointsUpdatedIntegrationEvent>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILoyaltyService _loyaltyService;
        private readonly ILogger<BuyerPointsUpdatedIntegrationEventHandler> _logger;

        public BuyerPointsUpdatedIntegrationEventHandler(ICouponRepository couponRepository,
        ILoyaltyService loyaltyService,
        ILogger<BuyerPointsUpdatedIntegrationEventHandler> logger)
        {
            _couponRepository = couponRepository;
            _loyaltyService = loyaltyService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(BuyerPointsUpdatedIntegrationEvent @event)
        {

            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation(
                    "-----Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
                    @event.Id, Program.AppName, @event);
                _logger.LogInformation(
                    "-----DEBUG integration event: {IntegrationEventId} with fields OrderId={OrderId}, BuyerId={BuyerId})",
                    @event.Id, @event.OrderId, @event.BuyerId);

                await _loyaltyService.SaveLoyalty(@event.BuyerId, @event.OrderItems);
            }
        }
    }
}