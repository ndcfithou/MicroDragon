using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.AggregatesModel.OrderAggregate
{
    public class OrderFill : Entity
    {
        public Guid OrderId { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal Fee { get; private set; }
        public DateTime FilledAt { get; private set; }

        protected OrderFill() { }

        public OrderFill(Guid orderId, decimal quantity, decimal price, decimal fee = 0)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Quantity = quantity;
            Price = price;
            Fee = fee;
            FilledAt = DateTime.UtcNow;
        }
    }
}
