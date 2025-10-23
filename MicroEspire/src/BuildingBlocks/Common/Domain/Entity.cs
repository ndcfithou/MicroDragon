using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public abstract class Entity
    {
        private int? _requestedHashCode;
        public virtual Guid Id { get; protected set; }

        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return Id == default;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity entity)
                return false;

            if (ReferenceEquals(this, entity))
                return true;

            if (GetType() != entity.GetType())
                return false;

            if (entity.IsTransient() || IsTransient())
                return false;

            return entity.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                _requestedHashCode ??= Id.GetHashCode() ^ 31;
                return _requestedHashCode.Value;
            }
            return base.GetHashCode();
        }

        public static bool operator ==(Entity? left, Entity? right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !(left == right);
        }
    }
}
