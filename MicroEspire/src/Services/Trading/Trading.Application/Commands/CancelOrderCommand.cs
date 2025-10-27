using Common.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Application.Commands
{
    public record CancelOrderCommand(Guid OrderId, Guid UserId) : IRequest<Result>;
}
