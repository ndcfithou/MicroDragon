using Common.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Application.Interface;
using Trading.Domain.AggregatesModel.OrderAggregate;

namespace Trading.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            ILogger<CreateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating order for user {UserId}, symbol {Symbol}",
                    request.UserId, request.Symbol);

                var order = new Order(
                    request.UserId,
                    request.Symbol,
                    request.Type,
                    request.Side,
                    request.Quantity,
                    request.Price,
                    request.StopPrice
                );

                _orderRepository.Add(order);
                await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                _logger.LogInformation("Order created successfully: {OrderId}", order.Id);

                return Result.Success(order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for user {UserId}", request.UserId);
                return Result.Failure<Guid>($"Failed to create order: {ex.Message}");
            }
        }
    }
}
