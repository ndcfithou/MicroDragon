using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Domain.AggregatesModel.WalletAggregate;

namespace WalletService.Domain.Events
{
    public record WalletWithdrawnDomainEvent(Wallet Wallet, decimal Amount, string Description) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
