using Northwind.Services.Events;

namespace Northwind.Models.Abstractions
{
	public abstract class AuditableBaseEntity
	{
		private readonly List<IDomainEvent> _domainEvents = new();
		public Guid Id { get; private set; }
		public Guid CreatedBy { get; private set; }
		public DateTime CreatedOn { get; private set; }
		public Guid? LastModifiedBy { get; private set; }
		public DateTime? LastModifiedOn { get; private set; }

		protected AuditableBaseEntity(
			Guid id,
			Guid createdBy,
			DateTime createdOn,
			Guid? lastModifiedBy,
			DateTime? lastModifiedOn)
		{
			Id = id;
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

			return Id == input.Id &&
				   CreatedBy == input.CreatedBy &&
				   CreatedOn == input.CreatedOn &&
				   LastModifiedBy == input.LastModifiedBy &&
				   LastModifiedOn == input.LastModifiedOn;
		}
	}
}