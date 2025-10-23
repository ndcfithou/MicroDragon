using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;
using Trading.Domain.AggregatesModel.OrderAggregate;

namespace Trading.Domain.Events
{
    public record OrderFilledDomainEvent(Order order) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
