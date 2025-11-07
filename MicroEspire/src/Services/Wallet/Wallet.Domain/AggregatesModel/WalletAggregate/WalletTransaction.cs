using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletService.Domain.AggregatesModel.WalletAggregate
{
    public class WalletTransaction : Entity
    {
        public Guid WalletId { get; private set; }
        public TransactionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public decimal BalanceAfter { get; private set; }
        public string Description { get; private set; }
        public string? Reference { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected WalletTransaction() { }

        public WalletTransaction(
            Guid walletId,
            TransactionType type,
            decimal amount,
            decimal balanceAfter,
            string description,
            string? reference = null)
        {
            Id = Guid.NewGuid();
            WalletId = walletId;
            Type = type;
            Amount = amount;
            BalanceAfter = balanceAfter;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Reference = reference;
            CreatedAt = DateTime.UtcNow;
        }
    }
    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal = 2,
        Fee = 3,
        Reward = 4,
        Transfer = 5
    }
}
