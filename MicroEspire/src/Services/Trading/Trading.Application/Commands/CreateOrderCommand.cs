using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application;
using MediatR;
using Trading.Domain.AggregatesModel.OrderAggregate;
namespace Trading.Application.Commands
{
    public record CreateOrderCommand(
    Guid UserId,
    string Symbol,
    OrderType Type,
    OrderSide Side,
    decimal Quantity,
    decimal Price,
    decimal? StopPrice = null
) : IRequest<Result<Guid>>;
}
