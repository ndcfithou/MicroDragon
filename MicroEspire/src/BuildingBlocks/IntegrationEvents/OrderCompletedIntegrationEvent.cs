using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationEvents
{
    public record OrderCompletedIntegrationEvent: IntegrationEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public string Symbol { get; init; } = string.Empty;
        public string Side { get; init; } = string.Empty;
        public decimal Quantity { get; init; }
        public decimal AveragePrice { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime CompletedAt { get; init; }
    }
}
