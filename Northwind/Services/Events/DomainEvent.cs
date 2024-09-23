
namespace Northwind.Services.Events
{
	internal sealed class DomainEvent : IDomainEvent
	{
		public Guid EventId { get; set; }
		public string EventName { get; set; }

		public DomainEvent(string eventName)
		{
			EventId = Guid.NewGuid();
			EventName = eventName;
		}

		public DomainEvent(Guid eventId, string eventName)
		{
			EventId = eventId;
			EventName = eventName;
		}
	}
}