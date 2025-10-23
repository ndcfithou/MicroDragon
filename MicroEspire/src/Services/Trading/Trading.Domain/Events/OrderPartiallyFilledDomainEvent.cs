using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.AggregatesModel.OrderAggregate;

namespace Trading.Domain.Events
{
    public record OrderPartiallyFilledDomainEvent(Order Order, decimal Quantity, decimal Price) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
