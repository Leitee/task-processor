using MediatR;

namespace TaskProcessor.Core.Shared;
public abstract class BaseEvent : INotification
{
	public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}