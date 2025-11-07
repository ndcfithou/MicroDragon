using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Application.DTOs;

namespace Trading.Application.Queries
{
    public record GetOrderQuery(Guid OrderId, Guid UserId) : IRequest<OrderDto?>;
}
