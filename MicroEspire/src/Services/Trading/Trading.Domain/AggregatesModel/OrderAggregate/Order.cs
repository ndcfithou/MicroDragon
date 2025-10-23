using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.Events;

namespace Trading.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public string Symbol { get; private set; }
        public OrderType Type { get; private set; }
        public OrderSide Side { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal? StopPrice { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal FilledQuantity { get; private set; }
        public decimal AveragePrice { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private readonly List<OrderFill> _fills = new();
        public IReadOnlyCollection<OrderFill> Fills => _fills.AsReadOnly();

        public Order(Guid userId, string symbol, OrderType type, OrderSide side,
                     decimal quantity, decimal price, decimal? stopPrice = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            if (price <= 0 && type != OrderType.Market)
                throw new ArgumentException("Price must be greater than zero for limit orders", nameof(price));

            Id = Guid.NewGuid();
            UserId = userId;
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Type = type;
            Side = side;
            Quantity = quantity;
            Price = price;
            StopPrice = stopPrice;
            Status = OrderStatus.Pending;
            FilledQuantity = 0;
            AveragePrice = 0;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new OrderCreatedDomainEvent(this));
        }

        public void Fill(decimal quantity, decimal price)
        {
            if (Status != OrderStatus.Pending && Status != OrderStatus.PartiallyFilled)
                throw new InvalidOperationException("Cannot fill a completed or cancelled order");

            if (quantity <= 0)
                throw new ArgumentException("Fill quantity must be greater than zero");

            var fill = new OrderFill(Id, quantity, price);
            _fills.Add(fill);

            FilledQuantity += quantity;
            AveragePrice = ((AveragePrice * (FilledQuantity - quantity)) + (price * quantity)) / FilledQuantity;
            UpdatedAt = DateTime.UtcNow;

            if (FilledQuantity >= Quantity)
            {
                Status = OrderStatus.Filled;
                CompletedAt = DateTime.UtcNow;
                AddDomainEvent(new OrderFilledDomainEvent(this));
            }
            else
            {
                Status = OrderStatus.PartiallyFilled;
                AddDomainEvent(new OrderPartiallyFilledDomainEvent(this, quantity, price));
            }
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Filled)
                throw new InvalidOperationException("Cannot cancel a filled order");

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order is already cancelled");

            Status = OrderStatus.Cancelled;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new OrderCancelledDomainEvent(this));
        }
    }

    public enum OrderType
    {
        Market = 1,
        Limit = 2,
        StopLoss = 3,
        StopLimit = 4
    }

    public enum OrderSide
    {
        Buy = 1,
        Sell = 2
    }

    public enum OrderStatus
    {
        Pending = 1,
        PartiallyFilled = 2,
        Filled = 3,
        Cancelled = 4,
        Rejected = 5
    }
}
