using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationEvents
{
    public record WalletBalanceChangedIntegrationEvent : IntegrationEvent
    {
        public Guid WalletId { get; init; }
        public Guid UserId { get; init; }
        public string Currency { get; init; } = string.Empty;
        public decimal OldBalance { get; init; }
        public decimal NewBalance { get; init; }
        public decimal AvailableBalance { get; init; }
        public string ChangeReason { get; init; } = string.Empty;
    }
}
