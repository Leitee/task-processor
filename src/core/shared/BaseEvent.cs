using MediatR;

namespace TaskProcessor.Core;
public abstract class BaseEvent : INotification
{
      public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}