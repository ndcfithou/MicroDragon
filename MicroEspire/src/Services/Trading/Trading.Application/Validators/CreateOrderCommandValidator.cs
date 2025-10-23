using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Application.Commands;
using Trading.Domain.AggregatesModel.OrderAggregate;

namespace Trading.Application.Validators
{
    public class CreateOrderCommandValidator :AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Symbol).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Quantity).GreaterThan(0);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .When(x => x.Type != OrderType.Market)
                .WithMessage("Price must be greater than zero for limit orders");

            RuleFor(x => x.StopPrice)
                .GreaterThan(0)
                .When(x => x.StopPrice.HasValue)
                .WithMessage("Stop price must be greater than zero when specified");
        }
    }
}
