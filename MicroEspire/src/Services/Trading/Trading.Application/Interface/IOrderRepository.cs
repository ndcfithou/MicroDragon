using Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.AggregatesModel.OrderAggregate;

namespace Trading.Application.Interface
{
    public interface IOrderRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Order Add(Order order);
        void Update(Order order);
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);
    }
}
