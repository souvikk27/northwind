using MediatR;

namespace Northwind.Services.Events
{
	public interface IDomainEvent : INotification
	{
		public Guid EventId { get; set; }
		public string EventName { get; set; }
	}
}