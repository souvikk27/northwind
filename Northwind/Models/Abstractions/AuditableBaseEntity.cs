using Northwind.Services.Events;

namespace Northwind.Models.Abstractions
{
    public abstract class AuditableBaseEntity
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        public Guid CreatedBy { get; internal set; }
        public DateTime CreatedOn { get; internal set; }
        public Guid? LastModifiedBy { get; internal set; }
        public DateTime? LastModifiedOn { get; internal set; }

        protected AuditableBaseEntity(
            Guid id,
            Guid createdBy,
            DateTime createdOn,
            Guid? lastModifiedBy,
            DateTime? lastModifiedOn)
        {
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            LastModifiedBy = lastModifiedBy;
            LastModifiedOn = lastModifiedOn;
        }

        protected AuditableBaseEntity()
        {
        }


        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }

        public virtual bool Equals(AuditableBaseEntity? input)
        {
            if (input is null)
            {
                return false;
            }

            return
                CreatedBy == input.CreatedBy &&
                CreatedOn == input.CreatedOn &&
                LastModifiedBy == input.LastModifiedBy &&
                LastModifiedOn == input.LastModifiedOn;
        }
    }
}