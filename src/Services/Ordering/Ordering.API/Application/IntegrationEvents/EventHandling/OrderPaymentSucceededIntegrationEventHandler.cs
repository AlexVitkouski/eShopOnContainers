using Ordering.API.Application.IntegrationEvents.Events;
using OrderItem = Ordering.API.Application.IntegrationEvents.Events.OrderItem;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.IntegrationEvents.EventHandling;
   
public class OrderPaymentSucceededIntegrationEventHandler :
    IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderPaymentSucceededIntegrationEventHandler> _logger;
    private readonly IEventBus _eventBus;
    private readonly IOrderRepository _orderRepository;

    public OrderPaymentSucceededIntegrationEventHandler(
        IMediator mediator,
        ILogger<OrderPaymentSucceededIntegrationEventHandler> logger,
        IOrderRepository orderRepository,
        IEventBus eventBus)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventBus = eventBus;
        _orderRepository = orderRepository;
    }

    public async Task Handle(OrderPaymentSucceededIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

            var command = new SetPaidOrderStatusCommand(@event.OrderId);

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                command.GetGenericTypeName(),
                nameof(command.OrderNumber),
                command.OrderNumber,
                command);

            await _mediator.Send(command);

            var order = await _orderRepository.GetAsync(@event.OrderId);
            
            List<OrderItem> items = order.OrderItems.Select(i => new OrderItem()
            {
                ProductId = i.ProductId, 
                UnitPrice = i.GetUnitPrice(), 
                UnitCount = i.GetUnits()
            }).ToList();
            
            IntegrationEvent buyerPointsUpdatedIntegrationEvent = new BuyerPointsUpdatedIntegrationEvent(@event.OrderId, order.GetBuyerId.Value, items);

            _eventBus.Publish(buyerPointsUpdatedIntegrationEvent);
        }
    }
}
