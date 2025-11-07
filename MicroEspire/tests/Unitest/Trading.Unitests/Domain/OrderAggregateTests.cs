using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Unitests.Domain
{
    public class OrderAggregateTests
    {
        [Fact]
        public void Create_Order_Should_Set_Pending_Status()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var symbol = "BTC/USDT";
            var quantity = 1.5m;
            var price = 50000m;

            // Act
            var order = new Order(userId, symbol, OrderType.Limit, OrderSide.Buy, quantity, price);

            // Assert
            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.Equal(userId, order.UserId);
            Assert.Equal(symbol, order.Symbol);
            Assert.Equal(quantity, order.Quantity);
            Assert.Equal(price, order.Price);
            Assert.Equal(0, order.FilledQuantity);
            Assert.Single(order.DomainEvents);
            Assert.IsType<OrderCreatedDomainEvent>(order.DomainEvents.First());
        }

        [Fact]
        public void Create_Order_With_Zero_Quantity_Should_Throw_Exception()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var symbol = "BTC/USDT";
            var quantity = 0m;
            var price = 50000m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Order(userId, symbol, OrderType.Limit, OrderSide.Buy, quantity, price));
        }

        [Fact]
        public void Fill_Order_Completely_Should_Update_Status_To_Filled()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var symbol = "BTC/USDT";
            var quantity = 1.0m;
            var price = 50000m;
            var order = new Order(userId, symbol, OrderType.Limit, OrderSide.Buy, quantity, price);

            // Act
            order.Fill(quantity, price);

            // Assert
            Assert.Equal(OrderStatus.Filled, order.Status);
            Assert.Equal(quantity, order.FilledQuantity);
            Assert.Equal(price, order.AveragePrice);
            Assert.NotNull(order.CompletedAt);
            Assert.Single(order.Fills);
        }

        [Fact]
        public void Fill_Order_Partially_Should_Update_Status_To_PartiallyFilled()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var symbol = "BTC/USDT";
            var quantity = 2.0m;
            var price = 50000m;
            var order = new Order(userId, symbol, OrderType.Limit, OrderSide.Buy, quantity, price);

            // Act
            order.Fill(1.0m, price);

            // Assert
            Assert.Equal(OrderStatus.PartiallyFilled, order.Status);
            Assert.Equal(1.0m, order.FilledQuantity);
            Assert.Null(order.CompletedAt);
        }

        [Fact]
        public void Cancel_Pending_Order_Should_Update_Status()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var symbol = "BTC/USDT";
            var quantity = 1.0m;
            var price = 50000m;
            var order = new Order(userId, symbol, OrderType.Limit, OrderSide.Buy, quantity, price);

            // Act
            order.Cancel();

            // Assert
            Assert.Equal(OrderStatus.Cancelled, order.Status);
            Assert.NotNull(order.CompletedAt);
        }

        [Fact]
        public void Cancel_Filled_Order_Should_Throw_Exception()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var symbol = "BTC/USDT";
            var quantity = 1.0m;
            var price = 50000m;
            var order = new Order(userId, symbol, OrderType.Limit, OrderSide.Buy, quantity, price);
            order.Fill(quantity, price);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.Cancel());
        }

        [Fact]
        public void Fill_Order_With_Multiple_Fills_Should_Calculate_Average_Price()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var symbol = "BTC/USDT";
            var quantity = 3.0m;
            var price = 50000m;
            var order = new Order(userId, symbol, OrderType.Limit, OrderSide.Buy, quantity, price);

            // Act
            order.Fill(1.0m, 50000m);
            order.Fill(1.0m, 51000m);
            order.Fill(1.0m, 49000m);

            // Assert
            Assert.Equal(OrderStatus.Filled, order.Status);
            Assert.Equal(3.0m, order.FilledQuantity);
            Assert.Equal(50000m, order.AveragePrice); // (50000 + 51000 + 49000) / 3
            Assert.Equal(3, order.Fills.Count);
        }
    }
