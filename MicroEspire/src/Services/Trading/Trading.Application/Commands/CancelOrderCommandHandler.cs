using Common.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Application.Interface;

namespace Trading.Application.Commands
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CancelOrderCommandHandler> _logger;

        public CancelOrderCommandHandler(IOrderRepository orderRepository, ILogger<CancelOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(request.OrderId);

                if (order == null)
                    return Result.Failure("Order not found");

                if (order.UserId != request.UserId)
                    return Result.Failure("Unauthorized to cancel this order");

                order.Cancel();
                await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                _logger.LogInformation("Order cancelled: {OrderId}", request.OrderId);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", request.OrderId);
                return Result.Failure($"Failed to cancel order: {ex.Message}");
            }
        }
    }
}
