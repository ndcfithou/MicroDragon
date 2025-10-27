using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Application.DTOs;
using Trading.Application.Interface;

namespace Trading.Application.Queries
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto?>
    {
        private readonly IOrderRepository _orderRepository;
        public GetOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            if (order == null || order.UserId != request.UserId)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Symbol = order.Symbol,
                Type = order.Type.ToString(),
                Side = order.Side.ToString(),
                Quantity = order.Quantity,
                Price = order.Price,
                StopPrice = order.StopPrice,
                Status = order.Status.ToString(),
                FilledQuantity = order.FilledQuantity,
                AveragePrice = order.AveragePrice,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                CompletedAt = order.CompletedAt
            };
        }
    }
}
