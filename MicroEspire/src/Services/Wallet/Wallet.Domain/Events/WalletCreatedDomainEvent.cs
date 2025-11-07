using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;
using WalletService.Domain.AggregatesModel.WalletAggregate;


namespace WalletService.Domain.Events
{
    public record WalletCreatedDomainEvent(Wallet wallet) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
