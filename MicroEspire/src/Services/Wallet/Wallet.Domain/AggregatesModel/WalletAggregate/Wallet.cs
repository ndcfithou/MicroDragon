using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Domain.Events;

namespace WalletService.Domain.AggregatesModel.WalletAggregate
{
    public class Wallet: Entity, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public string Currency { get; private set; }
        public decimal Balance { get; private set; }
        public decimal LockedBalance { get; private set; }
        public decimal AvailableBalance => Balance - LockedBalance;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<WalletTransaction> _transactions = new();
        public IReadOnlyCollection<WalletTransaction> Transactions => _transactions.AsReadOnly();
        protected Wallet() { }
        public Wallet(Guid userId, string currency)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            Balance = 0;
            LockedBalance = 0;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new WalletCreatedDomainEvent(this));
        }
        public void Deposit(decimal amount, string description, string? reference = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero", nameof(amount));

            Balance += amount;
            UpdatedAt = DateTime.UtcNow;

            var transaction = new WalletTransaction(
                Id,
                TransactionType.Deposit,
                amount,
                Balance,
                description,
                reference
            );

            _transactions.Add(transaction);
            AddDomainEvent(new WalletDepositedDomainEvent(this, amount, description));
        }
        public void Withdraw(decimal amount, string description, string? reference = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero", nameof(amount));

            if (AvailableBalance < amount)
                throw new InvalidOperationException("Insufficient available balance");

            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;

            var transaction = new WalletTransaction(
                Id,
                TransactionType.Withdrawal,
                -amount,
                Balance,
                description,
                reference
            );

            _transactions.Add(transaction);
            AddDomainEvent(new WalletWithdrawnDomainEvent(this, amount, description));
        }

        public void Lock(decimal amount, string reason)
        {
            if (amount <= 0)
                throw new ArgumentException("Lock amount must be greater than zero", nameof(amount));

            if (AvailableBalance < amount)
                throw new InvalidOperationException("Insufficient available balance to lock");

            LockedBalance += amount;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new WalletBalanceLockedDomainEvent(this, amount, reason));
        }
        public void Unlock(decimal amount, string reason)
        {
            if (amount <= 0)
                throw new ArgumentException("Unlock amount must be greater than zero", nameof(amount));

            if (LockedBalance < amount)
                throw new InvalidOperationException("Insufficient locked balance to unlock");

            LockedBalance -= amount;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new WalletBalanceUnlockedDomainEvent(this, amount, reason));
        }
    }
}
