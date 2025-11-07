using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Domain.AggregatesModel.WalletAggregate;

namespace WalletService.Domain.Events
{
    public record WalletBalanceUnlockedDomainEvent(Wallet Wallet, decimal Amount, string Reason) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
